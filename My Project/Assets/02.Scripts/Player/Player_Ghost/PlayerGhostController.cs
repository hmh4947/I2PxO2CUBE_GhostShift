using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : PlayerController
{
    #region Public Properties
    public float dashSpeed;
    public float dashDuration;
    public float jumpPower;
    public bool isDashing;

    #endregion
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
        if (!enabled) return;
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
            anim.SetBool("isJumping", true);
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
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
                    // 적 객체 삭제 후 플레이어 상태를 대쉬 상태로 변경 및 중력값 복구
                    Destroy(enemyObject);
                    SetPlayerStates(isSticking: false, isAbleDash: true);
                    rigid.gravityScale = 8.0f;
                    // 공격 사운드 2개중 하나 랜덤 출력
                    audio.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);
                    // 이펙트 생성 및 달라붙기 코루틴 종료
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
                    // 적이 빙의 가능한 객체일 경우
                    if(enemyType != EnemyType.NONE)
                    {
                        // 적 객체 삭제
                        Destroy(enemyObject);
                        // 플레이어 상태 초기화
                        SetPlayerStates();
                        // 적 타입에 따른 빙의 실행
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
    /*
     * 대쉬 시간동안 중력값을 0으로 설정 후에, 플레이어의 월드 좌표를 스크린 좌표로 변환한 후에 마우스의 스크린 좌표와의 뻴셈연산을
     * 통해 방향 벡터를 구하고 이를 단위벡터로 변경한다. 그리고 rigid의 velocity(속력)값을 구한 방향 벡터에 속력 값을 곱한 값으로 설정하여
     * 플레이어가 그 방향으로 대쉬할 수 있도록 설정한다.
     * 
     * Raycast를 사용해서 그 방향에 타일맵이 있다면 그냥 앞쪽으로 대쉬할 수 있도록 설정해봐야 할듯함.
     * */
    public IEnumerator Dash()
    {
        //대쉬가 가능할 경우
        if (isAbleDash)
        {
            // 대쉬 애니메이션 설정
            anim.SetBool("isDashing", true);
            // 대쉬 효과음 재생
            audio.PlayOneShot(dashSfx);
            // 플레이어 상태를 대쉬 상태로 변경
            SetPlayerStates(isDashing: true);
            // 현재 중력값 저장
            var originalGravityScale = rigid.gravityScale;
            // 중력값을 0으로 변경
            rigid.gravityScale = 0f;


            Vector2 playerToMouseVector = GetPlayerToMouseUnitVector();
            
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

            /* RaycastHit2D rayHit = Physics2D.Raycast(tr.position, playerToMouseVector, dashSpeed, LayerMask.GetMask("Platform"));
             Debug.DrawRay(transform.position, playerToMouseVector * dashSpeed, Color.blue);
             if (rayHit.collider != null && playerToMouseVector.y <= 0)
             {
                 playerToMouseVector.y = 0;
             }*/

            // 지정 방향으로 대쉬(가속력)
            rigid.velocity = playerToMouseVector * dashSpeed;
            // 충돌체크
            yield return new WaitForSeconds(dashDuration);

            // 대쉬 애니메이션 종료
            anim.SetBool("isDashing", false);

            //적에게 달라붙은 상태일 경우 바로 코루틴 종료
            if (isSticking)
            {
                audio.Stop();
                yield break;
            }

            // 플레이어 상태를 기본 상태로 변경
            SetPlayerStates();
            // 중력값 다시 설정.
            rigid.gravityScale = originalGravityScale;
        }
    }



    //달라붙기
    public IEnumerator StickTo()
    {
        // 달라붙기 애니메이션, 상태 설정 및 중력값, 속도 초기화 
        anim.SetBool("isSticking", true);
        SetPlayerStates(isSticking: true, isAbleDash: true);
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);


        //달라붙기가 종료될 때까지 대기
        yield return new WaitUntil(() => isSticking == false);

        // 달라붙기 애니메이션 종료 및 중력값 복구
        anim.SetBool("isSticking", false);
        rigid.gravityScale = 8.0f;
    }

    // 이펙트 생성 코루틴
    public IEnumerator GenerateEffects()
    {
        // 이펙트 게임 오브젝트 생성 및 카메라 쉐이크
        GameObject hitflash = Instantiate(hitEffect, tr.position, tr.rotation);
        CameraShake.Instance.OnShakeCamera();
        yield return new WaitForSeconds(0.2f);

        // 이펙트 게임 오브젝트 삭제
        Destroy(hitflash);
    }

}
