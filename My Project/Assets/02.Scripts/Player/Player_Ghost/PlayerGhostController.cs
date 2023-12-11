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

    private Vector3 groundPos;
    private Vector3 groundBoxSize;
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

    private void OnEnable()
    {
        SetScrCash();
        SetCashComponent();
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
        dashDuration = 0.2f;

        // Ground Check Pos
        groundPos = tr.position + new Vector3(0, -0.5f, 0);

        // Ground Check Bound
        groundBoxSize = new Vector3(1.2f, 0.6f, 0);

        isDashing = false;
        isAbleDash = true;
        isSticking = false;

        SetPlayerStates(isDashing, isAbleDash, isSticking);
    }
    public override void LoadResources()
    {
        jumpSfx = Resources.Load<AudioClip>("PlayerAudios/jump");
        dashSfx = Resources.Load<AudioClip>("PlayerAudios/dash");
        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
        attack2Sfx = Resources.Load<AudioClip>("PlayerAudios/attack2");
    }
    private void SetPlayerStates(bool isDashing, bool isAbleDash, bool isSticking)
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
            RaycastHit2D rayHit = Physics2D.BoxCast(groundPos, groundBoxSize, 0f, Vector3.down, 0.02f, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (isDashing)
                {
                    SetPlayerStates(isDashing, isAbleDash, isSticking);
                }
                else
                {
                    SetPlayerStates(isDashing, isAbleDash: true, isSticking);
                }
                
                anim.SetBool("isJumping", false);
            }
        }
    }

    // ���� ��Ҵ��� ���� üũ�� ���� �����
    private void OnDrawGizmos()
    {
        groundPos = tr.position + new Vector3(0, -0.8f, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundPos, groundBoxSize);
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
        if (isDashing) return;
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
                    SetPlayerStates(isDashing:false, isAbleDash: true, isSticking: false);
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
                        SetPlayerStates(isDashing:false, isAbleDash:true, isSticking:false);
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
            // �� Ÿ���� ��ۺ��� ���
            case EnemyType.GOGGLES:
                // �ٲ� Ÿ���� ��۷� ����
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
        SetPlayerStates(isDashing:false, isAbleDash: true, isSticking:false);
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
            SetPlayerStates(isDashing: true, isAbleDash:false, isSticking);
            // ���� �߷°� ����
            var originalGravityScale = rigid.gravityScale;
            if(rigid.gravityScale == 0)
            {
                originalGravityScale = 8.0f;
            }
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

            // ���� �������� �뽬(���ӷ�)
            rigid.velocity = playerToMouseVector * dashSpeed;
            //rigid.MovePosition(playerToMouseVector * dashSpeed);
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
            SetPlayerStates(isDashing:false, isAbleDash:true, isSticking:false);
            // �߷°� �ٽ� ����.
            rigid.gravityScale = originalGravityScale;
        }
    }
    //�޶�ٱ�
    public IEnumerator StickTo()
    {
        // �޶�ٱ� �ִϸ��̼�, ���� ���� �� �߷°�, �ӵ� �ʱ�ȭ 
        anim.SetBool("isSticking", true);
        SetPlayerStates(isDashing:false, isAbleDash: true, isSticking: true);
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
