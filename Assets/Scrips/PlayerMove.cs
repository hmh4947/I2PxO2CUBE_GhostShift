using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] bool isDashing;
    
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
   
    // Start is called before the first frame update
    void Start()
    {
        //Move Variable
        maxSpeed = 4.5f;

        //Jump Variable
        jumpPower = 22f;

        //Dash Variable
        dashSpeed = 25f;
        dashDuration = 0.2f;
        isDashing = false;
        
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
   
        //Jump
        if (Input.GetButtonDown("Jump")&& !anim.GetBool("isJumping"))
        {
            if(isDashing == false)
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
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Dash());
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
        if(isDashing == false)
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
                if (rayHit.distance < 0.5f)
                {
                    isDashing = false;
                    anim.SetBool("isJumping", false);
                }
                    
            }
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
           if(gameObject.TryGetComponent<Health>(out Health healthScr) == true)
            {
                healthScr.Damaged(1);
            }
        }
        if(collision.gameObject.tag == "Finish")
        {
            if (gameObject.TryGetComponent<Health>(out Health healthScr) == true)
            {
                healthScr.Healed(1);
            }
        }
    }

    //Dash
    private IEnumerator Dash()
    {
        if (isDashing == false)
        {
            isDashing = true;
            rigid.gravityScale = 0;
            Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;
            rigid.velocity = playerToMouseVector * dashSpeed;

            yield return new WaitForSeconds(dashDuration);
            rigid.gravityScale = 8;
        }
        
    }
}

