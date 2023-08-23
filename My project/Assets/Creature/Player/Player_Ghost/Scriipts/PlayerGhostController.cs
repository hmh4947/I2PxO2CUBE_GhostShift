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
    public bool isStickTo { get; set; }

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
            Debug.Log($"isStickTo: {isStickTo}");
            Debug.Log($"isAbleDash: { isAbleDash}");

        }
        //Jump
        if (Input.GetButtonDown("Jump")&& !anim.GetBool("isJumping"))
        {
            if(isDashing == false && isStickTo == false && isAbleDash && true)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }
            
        }


        //Stop Speed

        if (Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }

        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �޶���� �����ϰ�� �޶�ٱ⸦ �����ϰ� �뽬
                if (isStickTo)
                {
                    isStickTo = false;
                    rigid.gravityScale = 8.0f;
                    StopCoroutine(StickTo());
                }
                // �������� ��� ���Ǹ� �����ϰ� �뽬
                else if (isPossesing)
                {
                    isPossesing = false;
                    Init();
                    StartCoroutine(Dash());
                }
                // �ƴҰ�� �׳� �뽬
                StartCoroutine(Dash());
            }
            // ���콺 ��Ŭ�� �̺�Ʈ
            if (Input.GetMouseButtonDown(1))
            {
                // �޶�ٱ�, ���� ���� �ƴ� ��� ����
                if (isStickTo && !isPossesing)
                {
                    isStickTo = false;
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
                isStickTo = true;
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

    //�⺻ ����
    public void Init()
    {
        this.gameObject.SetActive(true);
        playerShield.SetActive(false);

        //Move Variable
        MaxSpeed = 7.5f;

        //Jump Variable
        jumpPower = 22f;

        //Dash Variable
        DashSpeed = 30f;
        DashDuration = 0.2f;
        isDashing = false;
        isAbleDash = true;


        //Possession Variable
        isPossesing = false;
        isStickTo = false;
    }

    //�⺻ ����2
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
            if (isDashing == false && isStickTo == false && isAbleDash == true)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }

        }
    }

    public void Move()
    {
        if (isDashing == false && isStickTo == false)
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


        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    //�뽬
    public IEnumerator Dash()
    {
        //�뽬�� ������ ���
        if (isAbleDash)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isDashing", true);
            isDashing = true;
            isAbleDash = false;
            isStickTo = false;
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
            rigid.velocity = playerToMouseVector * DashSpeed;

            // �浹üũ
            yield return new WaitForSeconds(DashDuration);

            anim.SetBool("isJumping", true);
            anim.SetBool("isDashing", false);


            //������ �޶���� ������ ��� �ٷ� �ڷ�ƾ ����
            if (isStickTo) 
                yield break;

            isDashing = false;
            rigid.gravityScale = originalGravityScale;
        }
    }

    

    //�޶�ٱ�
    public IEnumerator StickTo()
    {
        isDashing = false;
        isAbleDash = true;
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);

        //�޶�ٱⰡ ����� ������ ���
        yield return new WaitUntil(() => isStickTo == false);
        rigid.gravityScale = 8.0f;
    }

    //���� ĳ���ͷ� ����
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
