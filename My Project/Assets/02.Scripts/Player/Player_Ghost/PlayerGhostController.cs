using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : PlayerController
{
    // Field
    #region PlayerGhost Public Properties
    public float dashSpeed;
    public float dashDuration;
    public float jumpPower;
    #endregion
    #region PlayerGhost Private Properties
    private bool isDashing;
    private bool isAbleDash;
    private bool isSticking;
    private GameObject enemyObject;
    private EnemyType enemyType;
    #region PlayerGhost AudioClips
    private AudioClip jumpSfx;
    private AudioClip dashSfx;
    private AudioClip attack1Sfx;
    private AudioClip attack2Sfx;
    #endregion
    #endregion
    // Method
    #region PlayerGhost StartAndUpdate
    #region PlayerGhost Start
    // Start is called before the first frame update
    private void Start()
    {
        SetScrCash();
        SetCashComponent();
        LoadResources();
        Init();
    }
    #endregion
    #region PlayerGhost Update
    private void Update()
    {
        //Jump
        Jump();

        HandleMouseInput();
        
    }
    #endregion
    #region PlayerGhost FixedUpdate
    // Update is called once per frame
    void FixedUpdate()
    {
        //Move By Key Control(Move Speed)
        Move();

        //Landing Platform
        Gravity();
    }
    #endregion
    #endregion
    #region PlayerGhost Basic Settings
    //�⺻ ����
    public override void Init()
    {
        //Move Variable
        maxSpeed = 14.0f;

        //Jump Variable
        jumpPower = 22.0f;

        //Dash Variable
        dashSpeed = 30.0f;
        dashDuration = 0.2f;

        SetPlayerStates();
    }
    public override void LoadResources()
    {
        jumpSfx = Resources.Load<AudioClip>("PlayerAudios/jump");
        dashSfx = Resources.Load<AudioClip>("PlayerAudios/dash");
        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
        attack2Sfx = Resources.Load<AudioClip>("PlayerAudios/attack2");
    }
    private void SetPlayerStates(bool isDashing = false, bool isAbleDash = false, bool isSticking = false)
    {
        this.isDashing = isDashing;
        this.isAbleDash = isAbleDash;
        this.isSticking = isSticking;
    }
    #endregion
    #region PlayerGhost Behavior
    // �÷��̾� �߷�
    public override void Gravity()
    {
        if (rigid.velocity.y < 0)
        {
            anim.SetBool("isJumping", true);
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            /*RaycastHit2D rayHit = Physics2D.BoxCast(rigid.position, new Vector2(1.0f, 1.0f), 0, Vector3.down, 0, LayerMask.GetMask("Platform"));
            Debug.Log(rayHit.collider);*/
            /*if(Physics.CheckBox(groundCheck.position, groundBox, transform.rotation, groundMask)){
                Debug.Log("isGrounbded");
                SetPlayerStates(isAbleDash: true);
                anim.SetBool("isJumping", false);
            }*/
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.5f)
                {
                    SetPlayerStates(isAbleDash: true);
                    anim.SetBool("isJumping", false);
                }

            }
        }
    }
    // �÷��̾� ����
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            if (isDashing == false && isSticking == false && isAbleDash == true)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                audio.PlayOneShot(jumpSfx);
                anim.SetBool("isJumping", true);
            }

        }
    }
    // �÷��̾� �̵�
    public override void Move()
    {

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
        {
            anim.SetBool("isWalking", false);
        }

        else
        {
            anim.SetBool("isWalking", true);
        }

        if (isDashing == false && isSticking == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed) // Right Max Speed
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }
    }
    // ���콺 �̺�Ʈ
    public void HandleMouseInput()
    {
        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �޶���� �����ϰ�� �޶�ٱ⸦ �����ϰ� �뽬
                if (isSticking)
                {
                    // �� ��ü ���� �� �÷��̾� ���¸� �뽬 ���·� ���� �� �߷°� ����
                    Destroy(enemyObject);
                    SetPlayerStates(isSticking: false, isAbleDash: true);
                    rigid.gravityScale = 8.0f;
                    // ���� ���� 2���� �ϳ� ���� ���
                    audio.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);
                    // ����Ʈ ���� �� �޶�ٱ� �ڷ�ƾ ����
                    StartCoroutine(GenerateEffects());
                    StopCoroutine(StickTo());
                }
                // �ƴҰ�� �׳� �뽬
                StartCoroutine(Dash());
            }
            // ���콺 ��Ŭ�� �̺�Ʈ
            if (Input.GetMouseButtonDown(1))
            {
                if (isSticking)
                {
                    // ���� ���� ������ ��ü�� ���
                    if(enemyType != EnemyType.NONE)
                    {
                        // �� ��ü ����
                        Destroy(enemyObject);
                        // �÷��̾� ���� �ʱ�ȭ
                        SetPlayerStates();
                        // �� Ÿ�Կ� ���� ���� ����
                        playerScr.ChangePlayer(GetChangePlayerType(enemyType));
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!enabled) return;
        if (collider.tag == "Enemy")
        {
            if (isDashing)
            {
                Debug.Log("���� ����");
                if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemyObject = collider.gameObject;
                    enemy.Died();
                    enemyType = enemy.EnemyType;
                    tr.position = new Vector2(collider.transform.position.x, collider.transform.position.y + 0.5f);
                    StopCoroutine(Dash());
                    StartCoroutine(StickTo());
                }

            }
            else
            {
                Debug.Log("ü�� �ޱ�");
                healthScr.Damaged(1);
            }
        }
        if (collider.tag == "Bullet")
        {
            healthScr.Damaged(1);
            Destroy(collider.gameObject);
        }

    }
    // �����κ��� ������ �÷��̾� Ÿ���� �޾ƿ���
    public PlayerType GetChangePlayerType(EnemyType enemyType) // ���ڷ� �� Ÿ��(����) �޾ƿ���
    {
        PlayerType playerType = PlayerType.PLAYERGHOST;
        switch (enemyType)
        {               
            // �� Ÿ���� ���к��� ���
            case EnemyType.SHIELD:
                // �ٲ� Ÿ���� ���з� ����
                playerType = PlayerType.PLAYERSHIELD;
                break;
            // �� Ÿ���� ���ۺ��� ���
            case EnemyType.GOGGLES:
                // �ٲ� Ÿ���� ���۷� ����
                playerType = PlayerType.PLAYERGOGGLES;
                break;
            // �� Ÿ���� û�Һ��� ���
            case EnemyType.CLEANER:
                // �ٲ� Ÿ���� û�Һη� ����
                playerType = PlayerType.PLAYERCLEANER;
                break;
        }
        // �÷��̾� Ÿ�� ��ȯ
        return playerType;
    }

    public void ChangePlayerToGhost()
    {
        SetPlayerStates(isAbleDash: true);
        playerScr.IsPossesing = false;
        // �÷��̾ �ٽ� �������� ����� ����Ʈ ������ �Բ� �뽬�ϱ�.
        StartCoroutine(GenerateEffects());
        StartCoroutine(Dash());
        Debug.Log("���� �۵�");
    }
    #region PlayerGhost Coroutines
    // Coroutine

    //�뽬
    /*
     * �뽬 �ð����� �߷°��� 0���� ���� �Ŀ�, �÷��̾��� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ�� �Ŀ� ���콺�� ��ũ�� ��ǥ���� �y��������
     * ���� ���� ���͸� ���ϰ� �̸� �������ͷ� �����Ѵ�. �׸��� rigid�� velocity(�ӷ�)���� ���� ���� ���Ϳ� �ӷ� ���� ���� ������ �����Ͽ�
     * �÷��̾ �� �������� �뽬�� �� �ֵ��� �����Ѵ�.
     * 
     * Raycast�� ����ؼ� �� ���⿡ Ÿ�ϸ��� �ִٸ� �׳� �������� �뽬�� �� �ֵ��� �����غ��� �ҵ���.
     * */
    public IEnumerator Dash()
    {
        //�뽬�� ������ ���
        if (isAbleDash)
        {
            // �뽬 �ִϸ��̼� ����
            anim.SetBool("isDashing", true);
            // �뽬 ȿ���� ���
            audio.PlayOneShot(dashSfx);
            // �÷��̾� ���¸� �뽬 ���·� ����
            SetPlayerStates(isDashing: true);
            // ���� �߷°� ����
            var originalGravityScale = rigid.gravityScale;
            // �߷°��� 0���� ����
            rigid.gravityScale = 0f;


            Vector2 playerToMouseVector = GetPlayerToMouseUnitVector();
            
            //�뽬�� �� ���콺 ��ġ�� ���� ȸ��
            if (playerToMouseVector.x > 0)
            {
                if (spriteRenderer.flipX == false)
                    spriteRenderer.flipX = true;
            }
            else
            {
                if (spriteRenderer.flipX == true)
                    spriteRenderer.flipX = false;
            }

            /* RaycastHit2D rayHit = Physics2D.Raycast(tr.position, playerToMouseVector, dashSpeed, LayerMask.GetMask("Platform"));
             Debug.DrawRay(transform.position, playerToMouseVector * dashSpeed, Color.blue);
             if (rayHit.collider != null && playerToMouseVector.y <= 0)
             {
                 playerToMouseVector.y = 0;
             }*/

            // ���� �������� �뽬(���ӷ�)
            rigid.velocity = playerToMouseVector * dashSpeed;
            // �浹üũ
            yield return new WaitForSeconds(dashDuration);

            // �뽬 �ִϸ��̼� ����
            anim.SetBool("isDashing", false);

            //������ �޶���� ������ ��� �ٷ� �ڷ�ƾ ����
            if (isSticking)
            {
                audio.Stop();
                yield break;
            }

            // �÷��̾� ���¸� �⺻ ���·� ����
            SetPlayerStates();
            // �߷°� �ٽ� ����.
            rigid.gravityScale = originalGravityScale;
        }
    }
    //�޶�ٱ�
    public IEnumerator StickTo()
    {
        // �޶�ٱ� �ִϸ��̼�, ���� ���� �� �߷°�, �ӵ� �ʱ�ȭ 
        anim.SetBool("isSticking", true);
        SetPlayerStates(isSticking: true, isAbleDash: true);
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);


        //�޶�ٱⰡ ����� ������ ���
        yield return new WaitUntil(() => isSticking == false);

        // �޶�ٱ� �ִϸ��̼� ���� �� �߷°� ����
        anim.SetBool("isSticking", false);
        rigid.gravityScale = 8.0f;
    }
    // ����Ʈ ���� �ڷ�ƾ
    public IEnumerator GenerateEffects()
    {
        // ����Ʈ ���� ������Ʈ ���� �� ī�޶� ����ũ
        GameObject hitflash = Instantiate(hitEffect, tr.position, tr.rotation);
        CameraShake.Instance.OnShakeCamera();
        yield return new WaitForSeconds(0.2f);

        Destroy(hitflash);
    }
    #endregion
    #endregion
}