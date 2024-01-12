using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    // 플레이어 이동속도
    public float maxSpeed;

    public GameObject hitEffect;
    // 컴포넌트의 캐시를 처리할 변수들
    // 모든 플레이어 캐릭터가 공통적으로 가지고 있는 변수이므로, protected를 이용해 상속.
    // 메모리 소모를 최소화 하기 위함.
    // ------------------------------------------
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    protected CapsuleCollider2D playerCollider;
    protected Transform tr;
    protected new AudioSource audio;

    // ------------------------------------------


    // 스크립트 캐시처리
    protected Player playerScr;
    protected Health healthScr;

    void Start()
    {
        // 마우스 커서 밖으로 못나가게 하기
        Cursor.lockState = CursorLockMode.Confined;
    }
    public virtual void Init()
    {
        maxSpeed = 14.0f;
        /*hitflash = ObjectPooler.Instance.GetEffectObject();*/
    }
    public virtual void SetCashComponent() {
        // 플레이어 컴포넌트 캐쉬 처리
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponentInChildren<CapsuleCollider2D>();
        tr = GetComponent<Transform>();
        audio = GetComponent<AudioSource>();

    }
    public virtual void LoadResources() { }
    
    public virtual void SetScrCash()
    {
        //Script 캐쉬 처리
        playerScr = GetComponent<Player>();
        healthScr = GetComponent<Health>();
    }
    
    public virtual void Gravity()
    { }
    public virtual void Move() {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);


        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    // 마우스 입력 좌표로부터 플레이어 까지의 방향 벡터 구하기
    public virtual Vector2 GetPlayerToMouseUnitVector()
    {
        // 플레이어의 월드 좌표를 스크린 좌표로 변경
        Vector2 playerScreenPosition = transform.position;
        // 마우스 좌클릭시의 마우스 스크린 좌표
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 마우스 클릭 지점과 플레이어의 스크린 좌표의 방향 벡터
        Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;

        return playerToMouseVector;
    }
}
