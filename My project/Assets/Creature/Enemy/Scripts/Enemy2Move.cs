using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Move : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;  //행동지표를 결정할 변수 왼쪽 -1 , 오른쪽 1
   // public int Gravity;  //중력 값

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Wait();
        Invoke("Wait", 3); //주어진 시간이 지난 뒤, 지정된 함수를 실행하는 함수 -> ( "함수 이름" , 몇초뒤);

    }
 
    // Update is called once per frame
    void FixedUpdate()
    {   //Move
        rigid.velocity = new Vector2(nextMove, rigid.position.y);
        
        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove , rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        // DrawRay( ) : 에디터 상에서만 Ray를 그려주는 함수  (위치 , 쏘는 방향 ,레이 컬러(코드에는 녹색적용))
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));  // (위치 , 쏘는 방향 ,거리(안줘도 됀다) , Layer정보)
                                                                                                            //RayCastHit : Ray에 닿은 오브젝트, GetMask : 레이어 이름에 해당하는 정수값을 리턴하는 함
        if (rayHit.collider == null)
        {
            nextMove = nextMove *-1;
          //  Gravity -= 1;
            CancelInvoke();
            Invoke("Wait", 5);

        }

    }
    //재귀함수 : 자신을 스스로 호출하는 함수
    private List<int> exclusionList = new List<int>() { 0 }; //제외할 값
    void Wait()
    {
        nextMove = Random.Range(-1, 1);  //최대값에는 랜덤값포함

        while (exclusionList.Contains(nextMove))
        {
            nextMove = Random.Range(-1, 1);
        }

    }
}
//기본 움직임