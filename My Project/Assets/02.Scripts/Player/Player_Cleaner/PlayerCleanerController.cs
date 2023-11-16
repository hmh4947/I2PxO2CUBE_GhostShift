using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCleanerController : PlayerController
{
    public float swallowRange;
    // ��Ű�� ���϶� ����
    public bool isSwallowing;
    // ��Ŵ ������ ����
    public bool isSwallowed;

    //public GameObject enemyDiedObject; 
    // ��Ų ��(��Ÿ��)�� ������ ť
    private Queue<EnemyType> swalloedEnemy = new Queue<EnemyType>();
    // Start is called before the first frame update
    void Start()
    {
        SetScrCash();
        SetCashComponent();
        Init();
    }

    void FixedUpdate() {
        // �̵�
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 �Է� ����
        HandleMouseInput();
    }

    // �ʱ�ȭ
    public override void Init()
    {
        base.Init();

        isSwallowed = false;
        isSwallowing = false;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Enemy")
        {
            // ��Ű�� ���̶�� ������ X
            if (isSwallowing)
            {
                if(collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // ��Ű�� �� ��Ȱ��ȭ
                    isSwallowing = false;
                    // ��Ŵ ���� Ȱ��ȭ
                    isSwallowed = true;
                    // ��Ŵ �ִϸ��̼� Ȱ��ȭ
                    anim.SetBool("isSwallowed", true);
                    // �� ��ü ���ֱ�
                    Destroy(collider.gameObject);
                    // �� ��ü �߰�
                    swalloedEnemy.Enqueue(enemy.EnemyType);
                    
                }
               
            }
        }
        
    }
    private void HandleMouseInput() {
        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //��Ŵ ������ ��� �÷��̾� ���� �Ұ�
            if (!isSwallowed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // �÷��̾� �ٲٸ鼭 �뽬
                    ChangePlayer();
                }
            }

            if (isSwallowed)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Fire(swalloedEnemy.Dequeue());
                }
            }
            else
            {
                if (Input.GetMouseButton(1))
                {
                    Debug.Log("��Ű�� �õ�");
                    StartCoroutine(Swallow());
                }
                if (Input.GetMouseButtonUp(1))
                {

                    // ��Ű�� ���� �ƴҶ����� ��Ű�� ���߱�
                    if (!isSwallowing)
                    {
                        Debug.Log("��Ű�� �ߴ�");
                        StopCoroutine(Swallow());
                    }

                }
            }
           
        }
    }

    // �÷��̾� ����
    public void ChangePlayer()
    {
        Init();
        rigid.gravityScale = 8.0f;
        playerScr.ChangePlayer(PlayerType.PLAYERGHOST);
    }

    // ��Ű��
    IEnumerator Swallow()
    {

        //Physics2D.OverlapCircleAll : ������ ���� ����� �����Ϸ��� �ݰ� �̳��� ���� �ִ� �ݶ��̴����� �迭 ���·ι�ȯ�ϴ� �Լ�
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, swallowRange);
        // �ݶ��̴� �迭�� ��ȯ�ϸ鼭
        for(int i = 0; i < colliderArray.Length; i++)
        {
            // ������ ���ʹ̰� ������
            if(colliderArray[i].tag == "Enemy")
            {
                // ��Ű�� ��
                isSwallowing = true;
                // �� ��ü�� Die���·� ����
                if(colliderArray[i].TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.Died();
                }
                while(isSwallowing)
                {
                    // ���� �÷��̾��� ���� ���͸� ���ϰ�
                    Vector3 dir = (colliderArray[i].transform.position - transform.position).normalized;
                    // ���� �����ǿ� ���� ���͸� ���Ͽ� ���� �÷��̾��� ��ġ�� ������
                    dir = new Vector3(dir.x * 0.002f, dir.y * 0.002f, dir.z * 0.002f);
                    colliderArray[i].transform.position -= dir;

                    // �� ������ ����� �Ѱ��ֱ�
                    yield return null;
                }
                       
            }
        }
        // �������� ����Ǹ�(���콺 ������ Ȧ���� ����Ǹ�) �ڷ�ƾ ����
        yield break;
        
    }

    // ��Ų �� �߻�
    public void Fire(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.NONE:
                break;
            case EnemyType.SHIELD:
                break;
            case EnemyType.GOGGLES:
                break;
            case EnemyType.GUNNER:
                break;
            case EnemyType.CLEANER:
                break;
        }
        // ��Ŵ ���� ����
        isSwallowed = false;
        // �ִϸ��̼� ����(��Ŵ ���� ����)
        anim.SetBool("isSwallowed", false);
    }
}
