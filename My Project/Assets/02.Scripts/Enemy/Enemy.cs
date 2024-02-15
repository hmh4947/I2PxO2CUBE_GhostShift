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
    // enemy�� ���� ��ǥ
    private Transform enemyTr;
    // player�� ���� ��ǥ
    private Transform playerTr;

    // Animator �Ķ������ �ؽð� ����
    private readonly int hashWalk = Animator.StringToHash("isWalking");
    private readonly int hashDie = Animator.StringToHash("Die");
    // Enemy�� ��� ����
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

    // Enemy�� ���� ����
    public State state = State.MOVE;
    // ���� ����
    public float traceDist = 10.0f;
    // ���� �ӵ�
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

        // Enemy�� ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckEnemyState());
        // ���¿� ���� Enemy�� �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(EnemyAction());

        // �̵� ���� ���� ����
        moveDir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ������ �������� Enemy�� �ൿ ���¸� üũ
    protected virtual IEnumerator CheckEnemyState()
    {
        while (!isDied)
        {
            // 0.3�� ���� ����(���)�ϴ� ���� ������� �޽��� ������ �纸
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

    // �� ����� �˹�
    public void KnockBack(Vector2 dir)
    {
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.AddForce(new Vector2(knockBackPower * dir.x, 30.0f), ForceMode2D.Impulse);
        rigid.gravityScale = 8.0f;
    }
    public bool IsDied() { return this.isDied; }

}
