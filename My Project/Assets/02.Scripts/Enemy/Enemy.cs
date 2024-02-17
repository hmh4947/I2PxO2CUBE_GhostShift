using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public GameObject playerFoundUi;
    public GameObject canPossesUi;
    private GameObject bulletPrefab;

    protected Animator anim;
    protected CapsuleCollider2D coll;
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected new AudioSource audio;
    protected float knockBackPower;
    
    // 이동 방향
    private Vector2 moveDir;
    // 이동 방향 프로퍼티
    public Vector2 MoveDir { get; set; }
    // enemy의 현재 좌표
    private Transform enemyTr;
    // player의 현재 좌표
    private Transform playerTr;

    
    // 대기 여부
    private bool waiting = false;
    // Gunner 캐릭터 attackCount
    private int attackCount = 0;
    // Gunner 캐릭터 shootAudio
    private AudioClip shootSfx;
    // Gunner 캐릭터 총알발사 이펙트
    private GameObject shootFlashPrefab;
    // Animator 파라미터의 해시값 추출
    private readonly int hashWalk = Animator.StringToHash("isWalking");
    private readonly int hashDie = Animator.StringToHash("Die");
    // Enemy의 사망 여부
    [SerializeField]
    private bool isDied;

    public enum State
    {
        IDLE,
        MOVE,
        FOUND,
        TRACE,
        ESCAPE,
        ATTACK
    }

    // Enemy의 현재 상태
    public State state = State.IDLE;
    // 추적 범위
    public Vector3 sightRange = new Vector3(10, 1, 0);
    // 추적 속도
    public float traceSpeed = 1.5f;



    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        isDied = false;
        rigid = GetComponent<Rigidbody2D>();
        knockBackPower = 10.0f;
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

        // 총알 프리팹 로드
        bulletPrefab = Resources.Load<GameObject>("BulletPrefabs/Bullet");
        // 총알 이펙트 로드
        shootFlashPrefab = Resources.Load<GameObject>("Effects/FX_Fire");
        // 발사 효과음 로드
        shootSfx = Resources.Load<AudioClip>("EnemyAudios/gun_fire");
        // Enemy의 상태를 체크하는 코루틴 함수 호출
        StartCoroutine(CheckEnemyState());
        // 상태에 따라 Enemy의 행동을 수행하는 코루틴 함수 호출
        StartCoroutine(EnemyAction());

        // 이동 방향 랜덤 설정
        moveDir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        SetSpriteFlipX();

        if(enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
        {
            canPossesUi.SetActive(true);
        }
        else
        {
            canPossesUi.SetActive(false);
        }
        playerFoundUi.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    // 일정한 간격으로 Enemy의 행동 상태를 체크
    protected virtual IEnumerator CheckEnemyState()
    {
        while (!isDied)
        {
            // 0.3초 동안 중지(대기)하는 동안 제어권을 메시지 루프에 양보
            yield return new WaitForSeconds(0.3f);

            if(state == State.TRACE || state == State.ESCAPE || state == State.ATTACK)
            {
                continue;
            }


            if (CheckFront()) {
                state = State.IDLE;
                moveDir *= -1;
                waiting = true;
            }
            else
            {
                state = State.MOVE;
            }

            if (CheckPlayerInSight() && state != State.FOUND && !waiting)
            {
                state = State.FOUND;
            }

        }
    }

    protected virtual IEnumerator EnemyAction()
    {
        while (!isDied)
        {
            switch (state)
            {
                case State.IDLE:
                    anim.SetBool(hashWalk, false);
                    rigid.velocity = Vector2.zero;
                    yield return new WaitForSeconds(1.8f);
                    waiting = false;
                    SetSpriteFlipX();
                    break;
                case State.MOVE:
                    Move();
                    break;
                case State.FOUND:
                    // 플레이어 발견 상태 시 느낌표 스프라이트 띄우기
                    if (enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
                    {
                        canPossesUi.SetActive(false);
                    }
                    playerFoundUi.SetActive(true);
                    // 적 캐릭터 타입이 유령일 경우 TRACE 상태로 전환
                    if (enemyType == EnemyType.NONE || enemyType == EnemyType.CLEANER)
                    {
                        state = State.TRACE;
                    }
                    // 총병 타입일 경우 Attack 상태로 전환
                    else if(enemyType == EnemyType.GUNNER)
                    {
                        state = State.ATTACK;
                    }
                    // 그 외의경우 Escape 상태로 전환
                    else 
                    {
                        state = State.ESCAPE;
                    }

                    anim.SetBool(hashWalk, false);
                    rigid.velocity = Vector2.zero;
                    break;
                case State.TRACE:
                    if (enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
                    {
                        canPossesUi.SetActive(true);
                    }
                    playerFoundUi.SetActive(false);
                    Trace();
                    break;
                case State.ESCAPE:
                    if (enemyType == EnemyType.CLEANER || enemyType == EnemyType.GOGGLES || enemyType == EnemyType.SHIELD)
                    {
                        canPossesUi.SetActive(true);
                    }
                    playerFoundUi.SetActive(false);
                    Escape();
                    break;
                case State.ATTACK:
                    playerFoundUi.SetActive(false);
                    while (attackCount < 3 && !isDied)
                    {
                        Debug.Log("공격 진행");
                        Attack();
                        yield return new WaitForSeconds(0.6f);
                        attackCount++;
                    }
                    anim.SetBool("isShooting", false);
                    state = State.IDLE;
                    attackCount = 0;
                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    public void Move()
    {
        anim.SetBool(hashWalk, true);
        rigid.velocity = moveDir;
    }
    public bool CheckFront()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + moveDir.x, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1.5f, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null || rayHit.collider.CompareTag("NVDOBJECTS"))
        {
            return true;
        }
        rayHit = Physics2D.Raycast(frontVec, moveDir, 0.2f, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            return true;
        }
        
        return false;
    }
    public bool CheckPlayerInSight()
    {
        if (Mathf.Sign(sightRange.x) != Mathf.Sign(moveDir.x))
        {
            sightRange.x *= -1;
        }
        //Physics2D.OverlapAreaAll : 가상의 직사각형을 만들어 추출하려는 반경 이내에 들어와 있는 콜라이더들을 배열 형태로반환하는 함수
        Collider2D[] colliderArray = Physics2D.OverlapAreaAll(enemyTr.position, enemyTr.position + sightRange);
        // 콜라이더 배열을 순환하면서
        for (int i = 0; i < colliderArray.Length; i++)
        {
            // null이면 continue;
            if (colliderArray[i] == null) continue;
            // 주위에 에너미가 있으면
            if (colliderArray[i].tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
    public void Trace()
    {
        float distance = Vector2.Distance(playerTr.position, enemyTr.position);
        float dir = enemyTr.position.x - playerTr.position.x;

        if (((Mathf.Sign(moveDir.x) == Mathf.Sign(dir))) || (CheckFront() && enemyType != EnemyType.NONE))
        {
            moveDir *= -1;
        }
        anim.SetBool(hashWalk, true);
        SetSpriteFlipX();
        rigid.velocity = moveDir * traceSpeed;
    }
    public void Escape()
    {
        float distance = Vector2.Distance(playerTr.position, enemyTr.position);
        float dir = enemyTr.position.x - playerTr.position.x;

        if ((CheckPlayerInSight() && (Mathf.Sign(moveDir.x) != Mathf.Sign(dir)))|| CheckFront())
        {
            moveDir *= -1;
        }
        anim.SetBool(hashWalk, true);
        SetSpriteFlipX();
        rigid.velocity = moveDir * traceSpeed;
    }
    public void Attack()
    {
        anim.SetBool("isShooting", true);
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(enemyTr.position.x + moveDir.x, enemyTr.position.y, enemyTr.position.z), enemyTr.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(moveDir * 150.0f);
        audio.PlayOneShot(shootSfx);

        // 이펙트 게임 오브젝트 생성
        GameObject shootflash = Instantiate(shootFlashPrefab, new Vector3(enemyTr.position.x + moveDir.x, enemyTr.position.y-0.1f, enemyTr.position.z), enemyTr.rotation);

        // 0.2초뒤에 이펙트 삭제
        Destroy(shootflash, 0.2f);
}
    public void SetSpriteFlipX()
    {
        if (moveDir == Vector2.right)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    public void Died()
    {
        playerFoundUi.SetActive(false);
        anim.SetTrigger(hashDie);
        isDied = true;
        coll.isTrigger = true;
        rigid.velocity = new Vector2(0,0);
    }

    // 적 사망시 넉백
    public void KnockBack(Vector2 dir)
    {
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.AddForce(new Vector2(knockBackPower * dir.x, 30.0f), ForceMode2D.Impulse);
        rigid.gravityScale = 8.0f;
    }
    public bool IsDied() { return this.isDied; }

}
