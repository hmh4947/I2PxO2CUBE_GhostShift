using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;  //행동지표를 결정할 변수 왼쪽 -1 , 오른쪽 1
    public float distance;
    public GameObject Player;
    public GameObject Enemy;
    Transform enemy;

    float speed = 1.0f; //평소
    float speed_c = 1.5f;
    float axi = 1.0f; //방향

    public bool Player_detection = false;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        nextMove = 1; // 오른쪽 이동

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameObject.GetComponent<Enemy>().IsDied())
        {
            speed += 1;
            //Move
            enemy.Translate(new Vector3(1, 0, 0) * axi * speed);

            //Platform Check
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            // DrawRay( ) : 에디터 상에서만 Ray를 그려주는 함수  (위치 , 쏘는 방향 ,레이 컬러(코드에는 녹색적용))
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (위치 , 쏘는 방향 ,거리(안줘도 됀다) , Layer정보)
                                                                                                                //RayCastHit : Ray에 닿은 오브젝트, GetMask : 레이어 이름에 해당하는 정수값을 리턴하는 함
            if (rayHit.collider == null)
            {

                enemy.Translate(new Vector3(1, 0, 0) * axi *(-1) * speed);

            }
 
            /*
            Debug.DrawRay(frontVec, Vector3.forward, new Color(0, 1, 0));  //방향
                                                                           // DrawRay( ) : 에디터 상에서만 Ray를 그려주는 함수  (위치 , 쏘는 방향 ,레이 컬러(코드에는 녹색적용))
            RaycastHit2D rayHi1 = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (위치 , 쏘는 방향 ,거리(안줘도 됀다) , Layer정보)
            if (rayHit.collider == Player)
            {
                // 애니메이션 설정
                //Destroy
            }
            */


            float distance1 = Mathf.Abs(Enemy.transform.position.x - Player.transform.position.x);
            if (distance1 < 3.0f)
            {
                Player_detection = true;

                // Enemy가 Player의 반대방향으로 1.5배로 이동
                float direction = (Player.transform.position.x > Enemy.transform.position.x) ? 1.0f : -1.0f;
                Enemy.transform.position += new Vector3(direction * speed_c * Time.deltaTime, 0.0f, 0.0f);
            }
            else
            {
                Vector3 direction = (Player.transform.position - Enemy.transform.position).normalized;
                Enemy.transform.position += direction * speed_c * Time.deltaTime;
            }


        }
    }

}
   
