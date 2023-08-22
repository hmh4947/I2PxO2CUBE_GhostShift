using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float dashSpeed;
    public bool isDashing;

    [SerializeField] float jumpPower;
    [SerializeField] float dashDuration;
    [SerializeField] bool isAbleDash;
    [SerializeField] bool isStickTo; //�޶�ٰ� �ִ��� Ȯ���ϴ� bool ����
    [SerializeField] bool isPossesing;

    public GameObject playerGhost;
    public GameObject playerShield;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        //Move Variable
        maxSpeed = 7.5f;

        //Jump Variable
        jumpPower = 22f;

        //Dash Variable
        dashSpeed = 30f;
        dashDuration = 0.2f;
        isDashing = false;
        isStickTo = false;
        isAbleDash = true;

        rigid = GetComponent<Rigidbody2D>();
        Init();
    }

    private void Update()
    {
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
                // �ƴҰ�� �׳� �뽬
                StartCoroutine(Dash());
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (isStickTo)
                {
                    isStickTo = false;
                    playerGhost.SetActive(false);
                    playerShield.SetActive(true);
                    playerCollider = GetComponentInChildren<CapsuleCollider2D>();
                    anim = GetComponentInChildren<Animator>();
                    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                }
               
            }
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move By Key Control(Move Speed)
        if(isDashing == false && isStickTo == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed) // Right Max Speed
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }



        //Landing Platform
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
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
                if (gameObject.TryGetComponent<Health>(out Health healthScr) == true)
                {
                    healthScr.Damaged(1);
                }
            }
        }

    }

    //Dash
    private IEnumerator Dash()
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
            if(playerToMouseVector.x > 0)
            {
                if(spriteRenderer.flipX == false)
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
            if (isStickTo) 
                yield break;
            isDashing = false;
            rigid.gravityScale = originalGravityScale;
        }
    }

    public void Init()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponentInChildren<CapsuleCollider2D>();


        playerGhost.SetActive(true);
        playerShield.SetActive(false);
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

    /*
    private IEnumerator Possesion()
    {
        
    }
    */
}

