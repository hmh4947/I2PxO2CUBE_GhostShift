using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;  //행동지표를 결정할 변수 왼쪽 -1 , 오른쪽 1
    public float distace;
    public GameObject Player;
    public GameObject Enemy;
    public bool Detection = false;
    public bool Collison = false;
    public float speed = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        nextMove = -1; // 왼쪽 이동
        GetComponent<SpriteRenderer>().flipX = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameObject.GetComponent<Enemy>().IsDied())
        {
         
            distace = Vector2.Distance(Enemy.transform.position, Player.transform.position); //Player와 Enemy의 거리 재기 
            if (distace < 3.0f)
            {
                Detection = true;
                nextMove += 1 * (int)speed;
            }


           //rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

            //Platform Check
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            // DrawRay( ) : 에디터 상에서만 Ray를 그려주는 함수  (위치 , 쏘는 방향 ,레이 컬러(코드에는 녹색적용))
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (위치 , 쏘는 방향 ,거리(안줘도 됀다) , Layer정보)
                                                                                                                //RayCastHit : Ray에 닿은 오브젝트, GetMask : 레이어 이름에 해당하는 정수값을 리턴하는 함
            if (rayHit.collider == null)
            { 
                nextMove = nextMove * -1;
                GetComponent<SpriteRenderer>().flipX = true;

            }
        }
    }
   


}
//기본 움직임