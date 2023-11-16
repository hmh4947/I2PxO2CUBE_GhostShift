using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : PlayerController
{

    public float dashSpeed;
    public float dashDuration;
    public float jumpPower;
    public bool isDashing;

    [SerializeField]
    private bool isAbleDash;
    [SerializeField]
    private bool isSticking;

    private GameObject enemyObject;

    [SerializeField]
    private EnemyType enemyType;

    public AudioClip jumpSfx;
    public AudioClip dashSfx;
    public AudioClip attack1Sfx;
    public AudioClip attack2Sfx;

    // Start is called before the first frame update
    private void Start()
    {
        SetScrCash();
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


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            if (isDashing)
            {
                Debug.Log("정상 진입");
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
                Debug.Log("체력 달기");
                healthScr.Damaged(1);
            }
        }
        if (collider.tag == "Bullet")
        {
            healthScr.Damaged(1);
            Destroy(collider.gameObject);
        }

    }

    //기본 세팅
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

    private void SetPlayerStates(bool isDashing = false, bool isAbleDash = false, bool isSticking = false)
    {
        this.isDashing = isDashing;
        this.isAbleDash = isAbleDash;
        this.isSticking = isSticking;
    }

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

    // 마우스 이벤트
    public void HandleMouseInput()
    {
        // 일시정지 메뉴 클릭할 시에 되는걸 방지
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 달라붙은 상태일경우 달라붙기를 해제하고 대쉬
                if (isSticking)
                {
                    Destroy(enemyObject);
                    SetPlayerStates(isSticking: false, isAbleDash: true);
                    rigid.gravityScale = 8.0f;
                    // 공격 사운드 2개중 하나 랜덤 출력
                    audio.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);
                    StartCoroutine(GenerateEffects());
                    StopCoroutine(StickTo());
                }
                // 아닐경우 그냥 대쉬
                StartCoroutine(Dash());
            }
            // 마우스 우클릭 이벤트
            if (Input.GetMouseButtonDown(1))
            {
                if (isSticking)
                {
                    if(enemyType != EnemyType.NONE)
                    {
                        Destroy(enemyObject);
                        SetPlayerStates();
                        playerScr.ChangePlayer(GetChangePlayerType(enemyType));
                    }
                }
            }
        }
    }

    // 적으로부터 변경할 플레이어 타입을 받아오기
    public PlayerType GetChangePlayerType(EnemyType enemyType) // 인자로 적 타입(종류) 받아오기
    {
        PlayerType playerType = PlayerType.PLAYERGHOST;
        switch (enemyType)
        {               
            // 적 타입이 방패병일 경우
            case EnemyType.SHIELD:
                // 바꿀 타입을 방패로 설정
                playerType = PlayerType.PLAYERSHIELD;
                break;
            // 적 타입이 고글병일 경우
            case EnemyType.GOGGLES:
                // 바꿀 타입을 고글로 설정
                playerType = PlayerType.PLAYERGOGGLES;
                break;
            // 적 타입이 청소부일 경우
            case EnemyType.CLEANER:
                // 바꿀 타입을 청소부로 설정
                playerType = PlayerType.PLAYERCLEANER;
                break;
        }
        // 플레이어 타입 반환
        return playerType;
    }
    public void ChangePlayerToGhost()
    {
        SetPlayerStates(isAbleDash: true);
        playerScr.IsPossesing = false;
        // 플레이어를 다시 유령으로 변경시 이펙트 생성과 함께 대쉬하기.
        StartCoroutine(GenerateEffects());
        StartCoroutine(Dash());
        Debug.Log("정상 작동");
    }

    // Coroutine

    //대쉬
    public IEnumerator Dash()
    {
        //대쉬가 가능할 경우
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
            rigid.velocity = playerToMouseVector * dashSpeed;

            // 충돌체크
            yield return new WaitForSeconds(dashDuration);

            anim.SetBool("isJumping", true);
            anim.SetBool("isDashing", false);


            //적에게 달라붙은 상태일 경우 바로 코루틴 종료
            if (isSticking)
            {
                audio.Stop();
                yield break;
            }

            SetPlayerStates();
            rigid.gravityScale = originalGravityScale;
        }
    }



    //달라붙기
    public IEnumerator StickTo()
    {
        anim.SetBool("isSticking", true);
        SetPlayerStates(isSticking: true, isAbleDash: true);
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);


        //달라붙기가 종료될 때까지 대기
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
