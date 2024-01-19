using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goggles : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Collider2D collider;//충돌 감지
    public float moveSpeed = 1.00000f; //속도
    public float chase = 1.00005f;

    public bool check = true; //시간 지연

    public float dis;
    public float Detection_distance = 3.0f;
    public GameObject Player; //플레이어 객체
    public GameObject Enemy;  //객체
    public float axi; //방향

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
        Move();  // 이동
        Player_Detction(); // 플레이어 발견시
        Collision_Player(); //충돌처리
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

       
        //Platform Check -> raycast 사용
        Vector2 ray = new Vector2(Enemy.transform.position.x + axi * moveSpeed, Enemy.transform.position.y);
        Debug.DrawRay(ray, Vector3.down, new Color(0, 1, 0));
        // DrawRay( ) : 에디터 상에서만 Ray를 그려주는 함수  (위치 , 쏘는 방향 ,레이 컬러(코드에는 녹색적용))
        RaycastHit2D rayHit = Physics2D.Raycast(ray, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (위치 , 쏘는 방향 ,거리(안줘도 됀다) , Layer정보)
                                                                                                            //RayCastHit : Ray에 닿은 오브젝트, GetMask : 레이어 이름에 해당하는 정수값을 리턴하는 함
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
            //쫓기 || 피하기
        }
       

    }


    public void Collision_Player()
    {

    }

}
