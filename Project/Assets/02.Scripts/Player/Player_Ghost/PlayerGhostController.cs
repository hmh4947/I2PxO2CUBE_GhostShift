using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : MonoBehaviour, IPlayerController
{
    /*
     * ���� ������ ���� ��ȹ��.
    public enum State
    {
        IDLE,
        MOVE,
        DASH,
        JUMP,
        STICKING
    }
    */

    public float maxSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float jumpPower;
    public bool isDashing;

    [SerializeField]
    private bool isAbleDash;
    [SerializeField]
    private bool isSticking;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private CapsuleCollider2D playercollider;
    private Transform tr;

    public GameObject playerShield;
    public GameObject playerGoggles;
    public GameObject hitEffect;

    private GameObject enemyObject;
    private Health healthScr;
    private Player playerScr;

    [SerializeField]
    private EnemyType enemyType;

    private new AudioSource audio;

    public AudioClip jumpSfx;
    public AudioClip dashSfx;
    public AudioClip attack1Sfx;
    public AudioClip attack2Sfx;

    // Start is called before the first frame update
    private void Start()
    {
        SetCashComponent();
        Init();
    }

    private void Update()
    {
        //Jump
        Jump();

        HandleMouseInput();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move By Key Control(Move Speed)
        Move();

        //Landing Platform
        Gravity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (isDashing)
            {
                if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemyObject = collision.gameObject;
                    enemy.Died();
;                   enemyType = enemy.EnemyType;
                    tr.position = new Vector2(collision.transform.position.x, collision.transform.position.y + 0.5f);
                    StopCoroutine(Dash());
                    StartCoroutine(StickTo());
                }
                
            }
            else
            {
                healthScr.Damaged(1);
            }
        }

        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet")
        {
            healthScr.Damaged(1);
            Destroy(collider.gameObject);
        }

    }
        //�⺻ ����
        public void Init()
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

    //�޸� �Ҹ� ���̱� ���� ĳ�� ����
    public void SetCashComponent()
    {
        // �÷��̾� ������Ʈ ĳ�� ó��
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        playercollider= GetComponentInChildren<CapsuleCollider2D>();
        tr = GetComponent<Transform>();
        audio = GetComponent<AudioSource>();
        playerScr = GetComponentInParent<Player>();

        //Script ĳ�� ó��
        healthScr = gameObject.GetComponentInParent<Health>();
    }


    private void SetPlayerStates(bool isDashing = false, bool isAbleDash = false, bool isSticking = false)
    {
        this.isDashing = isDashing;
        this.isAbleDash = isAbleDash;
        this.isSticking = isSticking;
    }

    /*
    private void SetPlayerAnimation()
    {

    }
    */

    public void Gravity()
    {
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {
                    SetPlayerStates(isAbleDash: true);
                    anim.SetBool("isJumping", false);
                }

            }
        }
    }

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

    public void Move()
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
                    Destroy(enemyObject);
                    SetPlayerStates(isSticking: false, isAbleDash: true);
                    rigid.gravityScale = 8.0f;
                    audio.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);
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
                    if(enemyType != EnemyType.None)
                    {
                        Destroy(enemyObject);
                        SetPlayerStates();
                        ChangePlayer(enemyType);
                    }
                }
            }
        }
    }


    //���� ĳ���ͷ� ����
    public void ChangePlayer(EnemyType enemyType)
    {
        Init();
        rigid.gravityScale = 8.0f;

        switch(enemyType){
            case EnemyType.None:
                return;
            case EnemyType.Shield:
                playerShield.transform.position = tr.position;
                playerScr.IsPossesing = true;
                this.gameObject.SetActive(false);
                playerShield.SetActive(true);
                return;
            case EnemyType.Goggles:
                playerGoggles.transform.position = tr.position;
                playerScr.IsPossesing = true;
                this.gameObject.SetActive(false);
                playerGoggles.SetActive(true);
                return;

        }
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

    // Coroutine

    //�뽬
    public IEnumerator Dash()
    {
        //�뽬�� ������ ���
        if (isAbleDash)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isDashing", true);
            audio.PlayOneShot(dashSfx);
            SetPlayerStates(isDashing: true);
            var originalGravityScale = rigid.gravityScale;
            rigid.gravityScale = 0f;

            Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;

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
            rigid.velocity = playerToMouseVector * dashSpeed;

            // �浹üũ
            yield return new WaitForSeconds(dashDuration);

            anim.SetBool("isJumping", true);
            anim.SetBool("isDashing", false);


            //������ �޶���� ������ ��� �ٷ� �ڷ�ƾ ����
            if (isSticking)
            {
                audio.Stop();
                yield break;
            }

            SetPlayerStates();
            rigid.gravityScale = originalGravityScale;
        }
    }



    //�޶�ٱ�
    public IEnumerator StickTo()
    {
        anim.SetBool("isSticking", true);
        SetPlayerStates(isSticking: true, isAbleDash: true);
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);


        //�޶�ٱⰡ ����� ������ ���
        yield return new WaitUntil(() => isSticking == false);

        anim.SetBool("isSticking", false);
        rigid.gravityScale = 8.0f;
    }

    public IEnumerator GenerateEffects()
    {
        GameObject hitflash = Instantiate(hitEffect, tr.position, tr.rotation);
        CameraShake.Instance.OnShakeCamera();
        yield return new WaitForSeconds(0.2f);

        Destroy(hitflash);
    }

}
