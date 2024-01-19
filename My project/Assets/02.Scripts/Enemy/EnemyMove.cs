using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;  //�ൿ��ǥ�� ������ ���� ���� -1 , ������ 1
    public float distance;
    public GameObject Player;
    public GameObject Enemy;

    public GameObject prfHpBar;
    public GameObject canvas;

    RectTransform hpBar;

    public float height = 1.7f;
       
    //float speed =1.5f; 

    public bool Player_detection = false;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        nextMove = 1; // ������ �̵�

        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameObject.GetComponent<Enemy>().IsDied())
        {
            //Move
            rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

            //Platform Check
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            // DrawRay( ) : ������ �󿡼��� Ray�� �׷��ִ� �Լ�  (��ġ , ��� ���� ,���� �÷�(�ڵ忡�� �������))
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (��ġ , ��� ���� ,�Ÿ�(���൵ �´�) , Layer����)
                                                                                                                //RayCastHit : Ray�� ���� ������Ʈ, GetMask : ���̾� �̸��� �ش��ϴ� �������� �����ϴ� ��
            //Status
            Vector3 hpBarPos =
                Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
            hpBar.position = hpBarPos;


            if (rayHit.collider == null)
            {
                //Invoke("Wait", 2);
                nextMove = nextMove * -1;

            }



            /*
                         Debug.DrawRay(frontVec, Vector3.forward, new Color(0, 1, 0));
                         // DrawRay( ) : ������ �󿡼��� Ray�� �׷��ִ� �Լ�  (��ġ , ��� ���� ,���� �÷�(�ڵ忡�� �������))
                         RaycastHit2D rayHi1 = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (��ġ , ��� ���� ,�Ÿ�(���൵ �´�) , Layer����)
                         if (rayHit.collider == Player)
                         {

                         }



                         float distance1 = Mathf.Abs(Enemy.transform.position.x - Player.transform.position.x); 
                         if (distance1 < 3.0f)
                         {
                             Player_detection = true;

                             // Enemy�� Player�� �ݴ�������� 1.5��� �̵�
                             float direction = (Player.transform.position.x > Enemy.transform.position.x) ? 1.0f : -1.0f;
                             Enemy.transform.position += new Vector3(direction * speed * Time.deltaTime, 0.0f, 0.0f);
                         }
                         else
                         {
                             Vector3 direction= (Player.transform.position - Enemy.transform.position).normalized;
                             Enemy.transform.position += direction * speed * Time.deltaTime;
                         }
            */
        }
    }

  

          
        
    
}

//�⺻ ������