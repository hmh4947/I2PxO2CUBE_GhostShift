using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goggles : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Collider2D collider;//�浹 ����
    public float moveSpeed = 1.00000f; //�ӵ�
    public float chase = 1.00005f;

    public bool check = true; //�ð� ����

    public float dis;
    public float Detection_distance = 3.0f;
    public GameObject Player; //�÷��̾� ��ü
    public GameObject Enemy;  //��ü
    public float axi; //����

    bool Detection = false;  //animation parameter1
    bool Collison = false;  // animation parameter2

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        random_axi();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();  // �̵�
        Player_Detction(); // �÷��̾� �߽߰�
        Collision_Player(); //�浹ó��
    }

    private void random_axi() 
    {  
        axi = Random.Range(-0.1f, 0.2f);
        while (axi == 0)
        {
             axi = Random.Range(-0.1f,0.2f);
        }
    }

   
    
    private void Move()
    {
        transform.Translate(Vector2.right* axi * moveSpeed);
       // rigid.AddForce(transform.right * axi * moveSpeed,ForceMode2D.Force);
        if(axi < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(axi > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

       
        //Platform Check -> raycast ���
        Vector2 ray = new Vector2(Enemy.transform.position.x + axi * moveSpeed, Enemy.transform.position.y);
        Debug.DrawRay(ray, Vector3.down, new Color(0, 1, 0));
        // DrawRay( ) : ������ �󿡼��� Ray�� �׷��ִ� �Լ�  (��ġ , ��� ���� ,���� �÷�(�ڵ忡�� �������))
        RaycastHit2D rayHit = Physics2D.Raycast(ray, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (��ġ , ��� ���� ,�Ÿ�(���൵ �´�) , Layer����)
                                                                                                            //RayCastHit : Ray�� ���� ������Ʈ, GetMask : ���̾� �̸��� �ش��ϴ� �������� �����ϴ� ��
        if (rayHit.collider == null)
        {
            check = false;
            StartCoroutine(WaitForIt());
            axi *= -1;
        }

    }
    
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(2.0f);
        check = true;
    }
   
    public void Player_Detction()
    {
        dis = (Player.position - Enemy.position).sqrMagnitude;

        if(dis < Detection_distance)
        {
            //�ѱ� || ���ϱ�
        }
       

    }


    public void Collision_Player()
    {

    }

}
