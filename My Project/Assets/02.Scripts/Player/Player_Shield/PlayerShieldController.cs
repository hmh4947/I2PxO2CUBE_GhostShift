using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShieldController : PlayerController
{
    // Field
    #region PlayerShield Private Properties
    private float parryingDuration;
    private bool isParrying;
    public bool IsParrying { get; set; }
    private bool isDefending;
    private bool defended;
    // 쉴드 콜라이더 생성 포지션
    private Vector2 shieldPosition;
    // 패링 사운드
    private AudioClip swingSfx;

    #endregion
    #region PlayerShiled Public Properties
    public GameObject shield;
    #endregion
    #region PlayerShiled GetterAndSetter
    public void setDefended(bool defended)
    {
        this.defended = defended;
    }

    #endregion
    // Method
    #region PlayerShiled StartAndUpdate
    // Start is called before the first frame update
    void Start()
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
    void Update()
    {
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
    #endregion
    #region PlayerSheild Basic Settings
    //기본 세팅
    public override void Init()
    {
        //Move Variable
        maxSpeed = 7.5f;

        isParrying = false;
        parryingDuration = 0.25f;

        isDefending = false;
        shield.SetActive(false);

        defended = false;

        shieldPosition = new Vector3(0.8263f, 0, 0);
}

    // 리소스 로드
    public override void LoadResources()
    {
        swingSfx = Resources.Load<AudioClip>("PlayerAudios/swing");
    }
    #endregion
    #region PlayerShield Behavior
    //중력
    public override void Gravity()
    {
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {
                    anim.SetBool("isJumping", false);
                }

            }
        }
    }
    //이동
    public override void Move()
    {
        if (!isParrying && !isDefending)
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
            if(Input.GetAxisRaw("Horizontal") == 1)
            {
                spriteRenderer.flipX = true;
                shield.transform.localPosition = shieldPosition;
            }
            else
            {
                spriteRenderer.flipX = false;
                shield.transform.localPosition = shieldPosition * -1;
            }
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 스크립트가 비활성화 되어 있으면 return
        if (!enabled) return;

        // 충돌 객체가 Enemy
        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("쉴드 캐릭터 체력 달기");
            healthScr.Damaged(1);
        }
        if (collider.gameObject.tag == "Bullet")
        {
            if (!defended)
            {
                healthScr.Damaged(1);
                Destroy(collider.gameObject);
            }
        }

    }
    private void HandleMouseInput()
    {
        // 일시정지 메뉴 클릭할 시에 되는걸 방지
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 플레이어 바꾸면서 대쉬
                ChangePlayer();
            }
            // 마우스 우클릭 이벤트
            if (Input.GetMouseButtonDown(1))
            {
                if (isDefending)
                {
                    return;
                }
                if (!isParrying)
                {
                    StartCoroutine(Parrying());
                }

            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(Defending());
            }
            if (isDefending)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    isDefending = false;
            }
        }
    }
    #region PlayerShield Coroutines
    // 패링
    public IEnumerator Parrying()
    {
        anim.SetBool("isParrying", true);
        isParrying = true;
        audio.clip = swingSfx;
        audio.Play();
        shield.SetActive(true);

        yield return new WaitForSeconds(parryingDuration);

        isParrying = false;
        anim.SetBool("isParrying", false);
        shield.SetActive(false);
        if (defended)
            defended = false;
    }
    // 방어 모드
    public IEnumerator Defending()
    {
        // 방어 애니메이션 설정 및 방패 활성화
        anim.SetBool("isDefending", true);
        isDefending = true;
        shield.SetActive(true);

        yield return new WaitUntil(() => isDefending == false);

        // 방어 애니매이션 종료 및 방패 비활성화
        anim.SetBool("isDefending", false);
        isDefending = false;
        shield.SetActive(false);

    }
    #endregion
    //유령 캐릭터로 변경
    public void ChangePlayer()
    {
        Init();
        rigid.gravityScale = 8.0f;
        playerScr.ChangePlayer(PlayerType.PLAYERGHOST);
    }
    #endregion

}
