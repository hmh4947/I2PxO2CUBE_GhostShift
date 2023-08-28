using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : MonoBehaviour, IPlayerController
{
    public float MaxSpeed { get; set; }
    public float DashSpeed { get; set; }
    public bool isDashing { get; set; }
    public float jumpPower { get; set; }
    public float DashDuration { get; set; }

    public bool isAbleDash { get; set; }

    public bool isPossesing { get; set; }
    public bool isSticking { get; set; }

    public float animSpeed { get; set; }
    public Rigidbody2D rigid { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }
    public Animator anim { get; set; }
    public CapsuleCollider2D playerCollider { get; set; }

    public GameObject playerShield;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
        SetBasicComponent();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(anim);
            Debug.Log(rigid);
            Debug.Log($"Dashing: {isDashing}" );
            Debug.Log($"isSticking: {isSticking}");
            Debug.Log($"isAbleDash: { isAbleDash}");

        }

        //Jump
        Jump();

        // 일시정지 메뉴 클릭할 시에 되는걸 방지
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 달라붙은 상태일경우 달라붙기를 해제하고 대쉬
                if (isSticking)
                {
                    isSticking = false;
                    rigid.gravityScale = 8.0f;
                    StopCoroutine(StickTo());
                }
                // 빙의중일 경우 빙의를 해제하고 대쉬
                else if (isPossesing)
                {
                    isPossesing = false;
                    Init();
                    StartCoroutine(Dash());
                }
                // 아닐경우 그냥 대쉬
                StartCoroutine(Dash());
            }
            // 마우스 우클릭 이벤트
            if (Input.GetMouseButtonDown(1))
            {
                // 달라붙기, 빙의 중이 아닐 경우 빙의
                if (isSticking && !isPossesing)
                {
                    isSticking = false;
                    isPossesing = true;

                    ChangePlayer();
                }
               
            }

        }
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
                isSticking = true;
                this.transform.position = collision.transform.position;
                StopCoroutine(Dash());
                StartCoroutine(StickTo());
                Destroy(collision.gameObject);   
            }
            else
            {
                Health healthScr = gameObject.GetComponentInParent<Health>();
                {
                    healthScr.Damaged(1);
                }
            }
        }

    }

    //기본 세팅
    public void Init()
    {
        this.gameObject.SetActive(true);
        playerShield.SetActive(false);

        //Move Variable
        MaxSpeed = 14.0f;

        //Jump Variable
        jumpPower = 22.0f;

        //Dash Variable
        DashSpeed = 30.0f;
        DashDuration = 0.2f;
        isDashing = false;
        isAbleDash = true;


        //Possession Variable
        isPossesing = false;
        isSticking = false;
    }

    //기본 세팅2
    public void SetBasicComponent()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponentInChildren<CapsuleCollider2D>();
    }

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
                    isAbleDash = true;
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
                anim.speed = 1.0f;
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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
            anim.speed = 1.0f;
        }

        else
        {
            anim.SetBool("isWalking", true);
            if (!isDashing)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animSpeed = Random.Range(0.2f, 0.5f);
                    anim.speed = animSpeed;
                }

            }
        }

        if (isDashing == false && isSticking == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > MaxSpeed) // Right Max Speed
                rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < MaxSpeed * (-1)) // Left Max Speed
                rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);
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


        Debug.Log(anim.speed);
    }

    //대쉬
    public IEnumerator Dash()
    {
        //대쉬가 가능할 경우
        if (isAbleDash)
        {
            anim.speed = 1.0f;
            anim.SetBool("isJumping", false);
            anim.SetBool("isDashing", true);
            isDashing = true;
            isAbleDash = false;
            isSticking = false;
            var originalGravityScale = rigid.gravityScale;
            rigid.gravityScale = 0f;

            Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;

            //대쉬할 때 마우스 위치에 따라 회전
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
            rigid.velocity = playerToMouseVector * DashSpeed;

            // 충돌체크
            yield return new WaitForSeconds(DashDuration);

            anim.SetBool("isJumping", true);
            anim.SetBool("isDashing", false);


            //적에게 달라붙은 상태일 경우 바로 코루틴 종료
            if (isSticking) 
                yield break;

            isDashing = false;
            rigid.gravityScale = originalGravityScale;
        }
    }

    

    //달라붙기
    public IEnumerator StickTo()
    {
        animSpeed = 1.0f;
        anim.SetBool("isSticking", true);
        isDashing = false;
        isAbleDash = true;
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);

        //달라붙기가 종료될 때까지 대기
        yield return new WaitUntil(() => isSticking == false);
        anim.SetBool("isSticking", false);
        rigid.gravityScale = 8.0f;
        var fx_hit = GetComponentInChildren<ParticleSystem>();
        fx_hit.Play();
    }

    //쉴드 캐릭터로 변경
    public void ChangePlayer()
    {
        Init();
        rigid.gravityScale = 8.0f;
        playerShield.transform.position = this.transform.position;
        this.gameObject.SetActive(false);

        playerShield.SetActive(true);
    }

    public void ChangePlayerToGhost()
    {
        isPossesing = false;
        StartCoroutine(Dash());
    }

}
