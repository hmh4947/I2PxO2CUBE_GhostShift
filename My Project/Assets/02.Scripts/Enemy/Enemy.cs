using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType EnemyType;
    protected Animator anim;
    protected CapsuleCollider2D coll;
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected float knockBackPower;
    
    private Vector2 moveDir;
    // enemy의 현재 좌표
    private Transform enemyTr;
    // player의 현재 좌표
    private Transform playerTr;

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
        TRACE,
        ESCAPE,
        ATTACK,
        DIE
    }

    // Enemy의 현재 상태
    public State state = State.MOVE;
    // 추적 범위
    public float traceDist = 10.0f;
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

        // Enemy의 상태를 체크하는 코루틴 함수 호출
        StartCoroutine(CheckEnemyState());
        // 상태에 따라 Enemy의 행동을 수행하는 코루틴 함수 호출
        StartCoroutine(EnemyAction());

        // 이동 방향 랜덤 설정
        moveDir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 일정한 간격으로 Enemy의 행동 상태를 체크
    protected virtual IEnumerator CheckEnemyState()
    {
        while (!isDied)
        {
            // 0.3초 동안 중지(대기)하는 동안 제어권을 메시지 루프에 양보
            yield return new WaitForSeconds(0.3f);

            float distance = Vector2.Distance(playerTr.position, enemyTr.position);
            float dir = enemyTr.position.x - playerTr.position.x;

            if(distance <= traceDist && Mathf.Sign(moveDir.x) != Mathf.Sign(dir))
            {
                state = State.TRACE;
            }
            else
            {
                state = State.MOVE;
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
                    break;
                case State.MOVE:
                    Move();
                    break;
                case State.TRACE:
                    Trace();
                    break;
                case State.ATTACK:
                    break;
                case State.DIE:
                    break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
    public void Move()
    {
        anim.SetBool(hashWalk, true);
        if (moveDir == Vector2.right)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        rigid.velocity = moveDir;
        CheckFront();
    }
    public void CheckFront()
    {
        Vector2 frontVec = new Vector2(rigid.position.x + moveDir.x, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1.5f, LayerMask.GetMask("Platform"));
        if(rayHit.collider == null)
        {
            moveDir.x *= -1;
        }
    }
    public void Trace()
    {
        anim.SetBool(hashWalk, true);
        if (moveDir == Vector2.right)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        rigid.velocity = moveDir * traceSpeed;
    }
    public void Died()
    {
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
