using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : PlayerController
{
    // 대쉬속도
    public float dashSpeed;
    // 대쉬지속시간
    public float dashDuration;
    // 점프파워
    public float jumpPower;

    // 점프 중인지 확인하기 위한 변수
    private bool isJumping;
    // 대쉬 중인지 확인하기 위한 변수
    private bool isDashing;
    // 대쉬 가능 여부를 확인할 변수
    private bool isAbleDash;
    // 달라붙기 상태인지 확인하기 위한 변수
    private bool isSticking;
    // 지면에 있는디 확인하기 위한 변수
    private bool isGrounded = false;

    // 적 오브젝트를 저장할 변수
    private GameObject enemyObject;
    // 적 타입을 저장할 변수
    private EnemyType enemyType;

    // 바닥 충돌 체크를 확인할 변수
    private Vector3 groundPos;
    private Vector3 groundBoxSize;

    // 점프 효과음
    private AudioClip jumpSfx;
    // 대쉬 효과음
    private AudioClip dashSfx;
    // 공격 효과음들
    private AudioClip attack1Sfx;
    private AudioClip attack2Sfx;

    // Animator 파라미터의 해시값 추출
    private readonly int hashWalk = Animator.StringToHash("isWalking");
    private readonly int hashJump = Animator.StringToHash("isJumping");
    private readonly int hashDash = Animator.StringToHash("isDashing");
    private readonly int hashStick = Animator.StringToHash("isSticking");

    void Start()
    {
        // 캐시 세팅 함수 호출
        SetScrCash();
        SetCashComponent();

        // 리소스 로드 함수 호출
        LoadResources();
        // 플레이어 기본 능력치 세팅 함수 호출
        Init();
    }

    // 스크립트가 활성화 될때 마다 호출
    void OnEnable()
    {
        // 캐시 세팅 함수 호출
        SetScrCash();
        SetCashComponent();
        // 플레이어 기본 능력치 세팅 함수 호출
        Init();
    }


    void Update()
    {
        groundPos = tr.position + new Vector3(0, -0.8f, 0);
        // 중력 함수 호출
        Gravity();


        //Jump
        Jump();

        HandleMouseInput();

        // 움직임 체크 함수 호출
        Move();


    }


    // Update is called once per frame
    void FixedUpdate()
    {

    }


    //기본 세팅
    public override void Init()
    {
        // 이동 속도 설정
        maxSpeed = 14.0f;

        // 점프 파워 설정
        jumpPower = 22.0f;

        // 대쉬 지속시간 설정
        dashDuration = 0.2f;

        // 지면 충돌 체크 범위 시작지점 설정(플레이어의 좌표보다 y축으로 -0.5만큼 아래로 설정)
        groundPos = tr.position + new Vector3(0, -0.5f, 0);

        // 지면 충돌 체크 범위 설정(가로 0.8, 세로 0.6 길이의 상자)
        groundBoxSize = new Vector3(0.8f, 0.6f, 0);


        isJumping = false;
        isDashing = false;
        isAbleDash = false;
        isSticking = false;
    }
    public override void LoadResources()
    {
        // 점프 효과음 로드
        jumpSfx = Resources.Load<AudioClip>("PlayerAudios/jump");
        // 대쉬 효과음 로드
        dashSfx = Resources.Load<AudioClip>("PlayerAudios/dash");
        // 공격1 효과음 로드
        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
        // 공격2 효과음 로드
        attack2Sfx = Resources.Load<AudioClip>("PlayerAudios/attack2");
    }


    // 플레이어 중력
    public override void Gravity()
    {
        // 지면으로 박스 모양의 레이저를 쏴 지면에 닿았는지 체크
        RaycastHit2D rayHit = Physics2D.BoxCast(groundPos, groundBoxSize, 0f, Vector3.down, 0f, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            isGrounded = true;
            // 점프중이었다면 점프 상태 해제
            if (isJumping) isJumping = false;
            // 지면과 충돌했으면 추락 모션 해제
            anim.SetBool(hashJump, false);
            // 대쉬 가능 상태로 전이
            isAbleDash = true;
            return;
        }

        // 대쉬 중이면 추락 모션 설정 X
        if (isDashing) return;
        

        if (rigid.velocity.y < 0)
        {
            // 중력이 작용할 때 추락 모션 설정
            anim.SetBool(hashJump, true);
        }

        
    }

    // 땅에 닿았는지 범위 체크를 위한 기즈모
    private void OnDrawGizmos()
    {
        // 지면으로 박스 모양의 레이저를 쏴 지면에 닿았는지 체크
        RaycastHit2D rayHit = Physics2D.BoxCast(groundPos, groundBoxSize, 0f, Vector3.down, 0f, LayerMask.GetMask("Platform"));
        if (rayHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(tr.position, tr.lossyScale);
        }
        /*Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundPos, Vector3.down * 0.02f);*/
    }

    // 플레이어 점프
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            // 점프
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            // 점프 효과음 재생
            audio.PlayOneShot(jumpSfx);
            // 점프 애니메이션 재생
            anim.SetBool(hashJump, true);
            // 점프 상태로 전이
            isJumping = true;
            isGrounded = false;
        }
    }
    // 플레이어 이동
    public override void Move()
    {
        // 대쉬 중이거나 달라붙기 중일 경우 움직임 x
        if (isDashing || isSticking) return;

        // 이동속도가 0.5f 이하일 경우 이동 애니메이션 해제
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
        {
            anim.SetBool(hashWalk, false);
        }
        // 이동 애니메이션 설정
        else
        {
            anim.SetBool(hashWalk, true);
        }

        // 움직임 설정
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        
        if (rigid.velocity.x > maxSpeed) // 오른쪽 방향 최대 속력 설정
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 방향 최대 속력 설정
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);


        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 스프라이트 방향 변경
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

                    // 달라붙기 상태 종료
                    isSticking = false;

                    // 오류 체크
                    if (enemyObject == null)
                    {
                        Debug.LogError($"{0}: 삭제할 적 객체가 존재하지 않습니다.", this);
                        return;
                    }

                    // 적 객체 삭제
                    Destroy(enemyObject);
                    enemyObject = null;
                    // 공격 사운드 2개중 하나 랜덤 출력
                    audio.PlayOneShot(Random.Range(0, 2) == 1 ? attack1Sfx : attack2Sfx);

                    // 이펙트 생성
                    GenerateEffects();
                }
                // 대쉬
                StartCoroutine(Dash());
            }
            // 마우스 우클릭 이벤트
            if (Input.GetMouseButtonDown(1))
            {
                // 달라붙기 중일 경우만 우클릭 이벤트 실행
                if (isSticking)
                {
                    // 적이 빙의 가능한 객체일 경우
                    if(enemyType != EnemyType.NONE)
                    {
                        // 오류 체크
                        if (enemyObject == null) {
                            Debug.LogError($"{0}: 삭제할 적 객체가 존재하지 않습니다.", this);
                            return;
                        } 

                        // 적 객체 삭제
                        Destroy(enemyObject);

                        // 적 타입에 따른 빙의 실행
                        playerScr.ChangePlayer(GetChangePlayerType(enemyType));
                    }
                }
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 스크립트가 비활성화 중일 경우 return
        if (!enabled) return;

        // 적과 충돌했을 경우
        if (collider.CompareTag("Enemy"))
        {
            if (isDashing)
            {
                // 대쉬 중일 경우 달라붙기 실행
                Debug.Log("대쉬 후 달라붙기 진입");
                
                if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // 적 객체 저장
                    enemyObject = collider.gameObject;
                    // 적 상태를 die로 변경
                    enemy.Died();
                    // 적 객체 타입 저장
                    enemyType = enemy.EnemyType;
                    // 위치를 적 객체의 위로 보내기
                    tr.position = new Vector2(collider.transform.position.x, collider.transform.position.y + 0.5f);
                    // 대쉬 코루틴 종료
                    StopCoroutine(Dash());
                    // 달라붙기 코루틴 호출
                    StartCoroutine(StickTo());
                }
            }
            else
            {
                Debug.Log($"{0}: 적과 충돌하여 체력 달기", this);
                healthScr.Damaged(1);
            }
        }
        // 총알과 충돌했을 경우
        if (collider.CompareTag("Bullet"))
        {
            Debug.Log($"{0}: 총알과 충돌하여 체력 달기", this);
            healthScr.Damaged(1);
            Destroy(collider.gameObject);
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
        playerScr.IsPossesing = false;
        // 플레이어를 다시 유령으로 변경시 이펙트 생성과 함께 대쉬하기.
        GenerateEffects();
        StartCoroutine(Dash());
        Debug.Log($"{0}: 플레이어를 유령 타입으로 변환", this);
    }

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
            anim.SetBool(hashDash, true);
            // 대쉬 효과음 재생
            audio.PlayOneShot(dashSfx);
            // 플레이어 상태를 대쉬 상태로 변경
            isDashing = true;
            // 대쉬 불가능 상태로 전이
            isAbleDash = false;

            // 현재 중력값 저장
            var originalGravityScale = rigid.gravityScale;
            if(rigid.gravityScale == 0)
            {
                originalGravityScale = 8.0f;
            }
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

            // 지정 방향으로 대쉬(가속력)
            rigid.velocity = playerToMouseVector * dashSpeed;

            // 대쉬 지속시간 동안 제어권 양도
            yield return new WaitForSeconds(dashDuration);

            // 대쉬 상태 종료
            isDashing = false;
            // 대쉬 애니메이션 종료
            anim.SetBool(hashDash, false);
            // 플레이어 상태를 기본 상태로 변경
            //SetPlayerStates(isDashing:false, isAbleDash:false, isSticking:false);
            // 중력값 다시 설정.
            rigid.gravityScale = originalGravityScale;
        }
    }
    //달라붙기
    public IEnumerator StickTo()
    {
        isSticking = true;
        // 달라붙기 애니메이션, 상태 설정 및 중력값, 속도 초기화 
        anim.SetBool(hashStick, true);
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);

        // 달라붙기가 종료 될때까지 대기
        yield return new WaitUntil(() => isSticking == false);

        // 달라붙기 애니메이션 종료 및 중력값 복구
        anim.SetBool(hashStick, false);
        rigid.gravityScale = 8.0f;
    }
    // 이펙트 생성 코루틴
    public void GenerateEffects()
    {
        // 이펙트 게임 오브젝트 생성 및 카메라 쉐이크
        GameObject hitflash = Instantiate(hitEffect, tr.position, tr.rotation);
        CameraShake.Instance.OnShakeCamera();

        // 0.2초뒤에 이펙트 삭제
        Destroy(hitflash, 0.2f);
    }

}
