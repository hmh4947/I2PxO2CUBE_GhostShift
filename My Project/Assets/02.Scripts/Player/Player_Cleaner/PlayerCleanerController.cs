using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCleanerController : PlayerController
{
    // 삼키는 범위
    public float swallowRange;
    // 삼키는 중일때 변수
    public bool isSwallowing;
    // 삼킴 상태의 변수
    public bool isSwallowed;

    //public GameObject enemyDiedObject; 
    // 삼킨 적(적타입)을 저장할 큐
    private Queue<EnemyType> swalloedEnemy = new Queue<EnemyType>();
    // Start is called before the first frame update
    void Start()
    {
        SetScrCash();
        SetCashComponent();
        Init();
    }

    void FixedUpdate() {
        // 이동
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 입력 관리
        HandleMouseInput();
    }

    // 초기화
    public override void Init()
    {
        base.Init();

        isSwallowed = false;
        isSwallowing = false;
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Enemy타입의 객체와 충돌하게 된다면
        if(collider.tag == "Enemy")
        {
            // 삼키기 중이라면 데미지 X
            if (isSwallowing)
            {
                if(collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    // 삼키는 중 비활성화
                    isSwallowing = false;
                    // 삼킴 상태 활성화
                    isSwallowed = true;
                    // 삼킴 애니메이션 활성화
                    anim.SetBool("isSwallowed", true);
                    // 적 객체 없애기
                    Destroy(collider.gameObject);
                    // 발사할 적 객체 추가
                    swalloedEnemy.Enqueue(enemy.EnemyType);
                    
                }
               
            }
            // 삼키는 중이 아니라면
            else
            {
                // 데미지 입기
                healthScr.Damaged(1);
            }
        }

        
    }
    private void HandleMouseInput() {
        // 일시정지 메뉴 클릭할 시에 되는걸 방지
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //삼킴 상태일 경우 플레이어 변경 불가 하도록
            if (!isSwallowed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // 플레이어 바꾸면서 대쉬
                    ChangePlayer();
                }
            }

            // 삼킴 상태일 때 마우스 오른쪽 클릭 시 발사 공격 실행
            if (isSwallowed)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Fire(swalloedEnemy.Dequeue());
                }
            }
            // 삼킴 상태가 아닐때 마우스 오른쪽 클릭 시
            else
            {
                // 삼키기
                if (Input.GetMouseButton(1))
                {
                    Debug.Log("삼키기 시도");
                    StartCoroutine(Swallow());
                }
                //
                if (Input.GetMouseButtonUp(1))
                {

                    // 삼키는 중이 아닐때에만 삼키기 멈추기
                    if (!isSwallowing)
                    {
                        Debug.Log("삼키기 중단");
                        StopCoroutine(Swallow());
                    }

                }
            }
           
        }
    }

    // 플레이어 변경
    public void ChangePlayer()
    {
        Init();
        rigid.gravityScale = 8.0f;
        playerScr.ChangePlayer(PlayerType.PLAYERGHOST);
    }

    // 삼키기
    IEnumerator Swallow()
    {

        //Physics2D.OverlapCircleAll : 가상의 원을 만들어 추출하려는 반경 이내에 들어와 있는 콜라이더들을 배열 형태로반환하는 함수
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, swallowRange);
        // 콜라이더 배열을 순환하면서
        for(int i = 0; i < colliderArray.Length; i++)
        {
            // 주위에 에너미가 있으면
            if(colliderArray[i].tag == "Enemy")
            {
                // 삼키는 중
                isSwallowing = true;
                // 적 객체를 Die상태로 변경
                if(colliderArray[i].TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.Died();
                }
                while(isSwallowing)
                {
                    // 더 이상 에너미가 없으면 코루틴 종료
                    if (colliderArray[i] == null) {
                        Debug.Log($"enemy is null");
                        break;
                    }
                    
                    // 적과 플레이어의 방향 벡터를 구하고
                    Vector3 dir = (colliderArray[i].transform.position - transform.position).normalized;
                    // 적의 포지션에 방향 벡터를 더하여 적을 플레이어의 위치로 끌어당김
                    dir = new Vector3(dir.x * 0.05f, dir.y * 0.05f, dir.z * 0.05f);
                    colliderArray[i].transform.position -= dir;

                    // 한 프레임 제어권 넘겨주기
                    yield return null;
                }       
            }
        }
        // 끌어당김이 종료되면(마우스 오른쪽 홀딩이 종료되면) 코루틴 종료
        yield break;
        
    }

    // 삼킨 적 발사
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
        // 삼킴 상태 해제
        isSwallowed = false;
        // 애니메이션 설정(삼킴 상태 해제)
        anim.SetBool("isSwallowed", false);
    }
}
