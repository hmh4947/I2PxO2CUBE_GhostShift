using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCleanerController : PlayerController
{
    // ��Ű�� ����
    public float swallowRange;
    // ��Ű�� ���϶� ����
    public bool isSwallowing;
    // ��Ŵ ������ ����
    public bool isSwallowed;

    //public GameObject enemyDiedObject; 
    // ��Ų ��(��Ÿ��)�� ������ ť
    private Queue<EnemyType> swalloedEnemy = new Queue<EnemyType>();

    public Sprite[] enemyDiedSprites;

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
        // EnemyŸ���� ��ü�� �浹�ϰ� �ȴٸ�
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
                    // �߻��� �� ��ü �߰�
                    swalloedEnemy.Enqueue(enemy.EnemyType);
                    
                }
               
            }
            // ��Ű�� ���� �ƴ϶��
            else
            {
                // ������ �Ա�
                healthScr.Damaged(1);
            }
        }

        
    }
    private void HandleMouseInput() {
        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //��Ŵ ������ ��� �÷��̾� ���� �Ұ� �ϵ���
            if (!isSwallowed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // �÷��̾� �ٲٸ鼭 �뽬
                    ChangePlayer();
                }
            }

            // ��Ŵ ������ �� ���콺 ������ Ŭ�� �� �߻� ���� ����
            if (isSwallowed)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Fire(swalloedEnemy.Dequeue());
                }
            }
            // ��Ŵ ���°� �ƴҶ� ���콺 ������ Ŭ�� ��
            else
            {
                // ��Ű��
                if (Input.GetMouseButton(1))
                {
                    Debug.Log("��Ű�� �õ�");
                    StartCoroutine(Swallow());
                }
                //
                if (Input.GetMouseButtonUp(1))
                {

                    // ��Ű�� ���� �ƴҶ����� ��Ű�� ���߱�
                    if (!isSwallowing)
                    {
                        Debug.Log("��Ű�� �ߴ�");
                        anim.SetBool("isSwallowing", false);
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
        // ��Ű�� �ִϸ��̼� ����
        anim.SetBool("isSwallowing", true);
        //Physics2D.OverlapCircleAll : ������ ���� ����� �����Ϸ��� �ݰ� �̳��� ���� �ִ� �ݶ��̴����� �迭 ���·ι�ȯ�ϴ� �Լ�
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, swallowRange);
        // �ݶ��̴� �迭�� ��ȯ�ϸ鼭
        for(int i = 0; i < colliderArray.Length; i++)
        {
            // null�̸� continue;
            if (colliderArray[i] == null) continue;
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
                    // �� �̻� ���ʹ̰� ������ �ڷ�ƾ ����
                    if (colliderArray[i] == null) {
                        Debug.Log($"enemy is null");
                        break;
                    }
                    
                    // ���� �÷��̾��� ���� ���͸� ���ϰ�
                    Vector3 dir = (colliderArray[i].transform.position - transform.position).normalized;
                    // ���� �����ǿ� ���� ���͸� ���Ͽ� ���� �÷��̾��� ��ġ�� ������
                    dir = new Vector3(dir.x * 0.01f, dir.y * 0.01f, dir.z * 0.01f);
                    colliderArray[i].transform.position -= dir;

                    // �� ������ ����� �Ѱ��ֱ�
                    yield return null;
                }       
            }
        }
        // �������� ����Ǹ�(���콺 ������ Ȧ���� ����Ǹ�) �ִϸ��̼� �� �ڷ�ƾ ����,

        if (isSwallowed) {
            anim.SetBool("isSwallowing", false);
            yield break;
        } 
        
    }

    // ��Ų �� �߻�
    public void Fire(EnemyType enemyType)
    {
        // ��Ŵ ���� ����
        isSwallowed = false;
        // �ִϸ��̼� ����(��Ŵ ���� ����)
        anim.SetBool("isSwallowed", false);

        Sprite enemyDiedBullet = enemyDiedSprites[0];
        // �߻� ���� �� �ִϸ��̼� ����
        anim.SetBool("Firing", true);

        switch (enemyType)
        {
            case EnemyType.NONE:
                enemyDiedBullet = enemyDiedSprites[0];
                break;
            case EnemyType.SHIELD:
                enemyDiedBullet = enemyDiedSprites[1];
                break;
            case EnemyType.GOGGLES:
                enemyDiedBullet = enemyDiedSprites[2];
                break;
            case EnemyType.GUNNER:
                enemyDiedBullet = enemyDiedSprites[3];
                break;
            case EnemyType.CLEANER:
                enemyDiedBullet = enemyDiedSprites[4];
                break;
        }

        // �÷��̾��� ���� ��ǥ�� ��ũ�� ��ǥ�� ����
        Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        // ���콺 ��Ŭ������ ���콺 ��ũ�� ��ǥ
        Vector2 mouseScreenPosition = Input.mousePosition;
        // ���콺 Ŭ�� ������ �÷��̾��� ��ũ�� ��ǥ�� ���� ����
        Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;


        //GameObject enemyBullet = Instantiate(enemyDiedBullet, tr.position, tr.rotation);
        // �ִϸ��̼� ����
        anim.SetBool("Firing", false);
        return;
    }
}
