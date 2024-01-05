using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCleanerController : PlayerController
{
    // 청소부 캐릭터 public 변수
    #region PlayerCleaner Public Properties
    // 삼키는 속도
    public float swallowSpeed;
    #endregion
    // 청소부 캐릭터 private 변수
    #region PlayerCleaner Private Properties
    // 삼키는 범위
    private Vector3 swallowRange;
    // 삼키는 중일때 변수
    private bool isSwallowing;
    // 삼킴 상태의 변수
    private bool isSwallowed;
    // 삼킨 적(적타입)을 저장할 큐
    private Queue<EnemyType> swalloedEnemy = new Queue<EnemyType>();
    // enemyDied 스프라이트를 저장할 배열 생성
    private Sprite[] enemyDiedBulletSprites = new Sprite[5];
    // 총알 발사는 오브젝트 풀링으로 구현해보기
    private GameObject enemyDiedBulletPrefab;

    // Audio Clips
    private AudioClip attack1Sfx;
    private AudioClip attack2Sfx;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SetScrCash();
        SetCashComponent();
        Init();
        LoadResources();

    }

    private void OnEnable()
    {
        SetScrCash();
        SetCashComponent();
        Init();

    }
    void FixedUpdate() {
        // 삼키는 중이 아닐 경우
        if(!isSwallowing)
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

        swallowRange = new Vector3(8.0f, 1.0f, 0.0f);
        

    }

    public override void LoadResources()
    {
        // 플레이어 총알 받아오기
        enemyDiedBulletPrefab = Resources.Load<GameObject>("BulletPrefabs/PlayerBullet");
        // 유령 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[0] = Resources.Load<Sprite>("EnemyImages/Enemy_Ghost/Enemy_ghost_dead");
        // 방패병 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[1] = Resources.Load<Sprite>("EnemyImages/Enemy_Shield/Enemy_shield_dead");
        // 고글 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[2] = Resources.Load<Sprite>("EnemyImages/Enemy_Goggles/Enemy_goggles_dead");
        // 거너 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[3] = Resources.Load<Sprite>("EnemyImages/Enemy_Gunner/Enemy_gun_dead");
        // 청소부 타입 적 사망 스프라이트 로드
        enemyDiedBulletSprites[4] = Resources.Load<Sprite>("EnemyImages/Enemy_Gunner/Enemy_gun_dead");

        attack1Sfx = Resources.Load<AudioClip>("PlayerAudios/attack1");
        attack2Sfx = Resources.Load<AudioClip>("PlayerAudios/attack2");
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!enabled) return;
        // Enemy타입의 객체와 충돌하게 된다면
        if (collider.tag == "Enemy")
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
                Debug.Log("청소부 캐릭터 데미지 입기");
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

            // 삼킴 상태가 아닐때 마우스 오른쪽 클릭 시
            if (!isSwallowed /*|| !anim.GetCurrentAnimatorStateInfo(0).IsName("Fire")*/)
            {
                // 삼키기
                if (Input.GetMouseButtonDown(1))
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
                        anim.SetBool("isSwallowing", false);
                        StopCoroutine(Swallow());
                    }

                }
            }
            // 삼킴 상태일 때 마우스 오른쪽 클릭 시 발사 공격 실행
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Fire(swalloedEnemy.Dequeue());
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
        // 삼키는 애니메이션 설정
        anim.SetBool("isSwallowing", true);

        // 플레이어 이동 방향에 따른 삼키는 방향 설정
        if((spriteRenderer.flipX == false && swallowRange.x > 0) || (spriteRenderer.flipX == true && swallowRange.x < 0))
        {
            swallowRange.x *= -1;
        }

        //Physics2D.OverlapAreaAll : 가상의 직사각형을 만들어 추출하려는 반경 이내에 들어와 있는 콜라이더들을 배열 형태로반환하는 함수
        Collider2D[] colliderArray = Physics2D.OverlapAreaAll(transform.position, transform.position + swallowRange);
        // 콜라이더 배열을 순환하면서
        for(int i = 0; i < colliderArray.Length; i++)
        {
            // null이면 continue;
            if (colliderArray[i] == null) continue;
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
                    // 더 이상 에너미가 없으면 반복문 종료
                    if (colliderArray[i] == null || isSwallowed) {
                        Debug.Log($"enemy is null");
                        break;
                    }
                    
                    // 적과 플레이어의 방향 벡터를 구하고
                    Vector3 dir = (colliderArray[i].transform.position - transform.position).normalized;
                    // 적의 포지션에 방향 벡터를 더하여 적을 플레이어의 위치로 끌어당김
                    dir = new Vector3(dir.x * 2.0f, dir.y * 2.0f, dir.z * 2.0f) ;
                    // 끌어당기는 속도 설정
                    dir *= swallowSpeed * Time.deltaTime;
                    // 끌어당기기
                    colliderArray[i].transform.position -= dir;

                    // 한 프레임 제어권 넘겨주기
                    yield return null;
                }       
            }
        }
        // 끌어당김이 종료되면(마우스 오른쪽 홀딩이 종료되면) 애니메이션 및 코루틴 종료,
        if (isSwallowed) {
            anim.SetBool("isSwallowing", false);
            yield break;
        } 
        
    }

    // 삼킨 적 발사
    public void Fire(EnemyType enemyType)
    {
        anim.Play("player_cleaner_fire", -1);
        audio.PlayOneShot(attack1Sfx);
        // 흡수한 적 총알 객체 생성
        GameObject enemyDiedBullet = Instantiate(enemyDiedBulletPrefab, tr.position, tr.rotation);
        // 스프라이트 받아오기
        SpriteRenderer enemyDiedBulletSprite = enemyDiedBullet.GetComponent<SpriteRenderer>();

        switch (enemyType)
        {
            // 흡수한 적 객체가 유령타입일 경우 총알을 유령 Died 스프라이트로 변경
            case EnemyType.NONE:
                enemyDiedBulletSprite.sprite= enemyDiedBulletSprites[0];
                break;
            // 흡수한 적 객체가 방패타입일 경우 총알을 방패병 Died 스프라이트로 변경
            case EnemyType.SHIELD:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[1];
                break;
            // 흡수한 적 객체가 고글타입일 경우 총알을 고글 Died 스프라이트로 변경
            case EnemyType.GOGGLES:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[2];
                break;
            // 흡수한 적 객체가 거너타입일 경우 총알을 거너 Died 스프라이트로 변경
            case EnemyType.GUNNER:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[3];
                break;
            // 흡수한 적 객체가 청소부타입일 경우 총알을 청소부 Died 스프라이트로 변경
            case EnemyType.CLEANER:
                enemyDiedBulletSprite.sprite = enemyDiedBulletSprites[4];
                break;
        }



        // 마우스 클릭 지점과 플레이어의 스크린 좌표의 방향 벡터
        Vector2 playerToMouseVector = GetPlayerToMouseUnitVector();

        // 총알 발사: 총알 프리팹으로 부터 RigidBody를 받아와 마우스 클릭 지점으로 힘을 주어 총알 발사.
        Rigidbody2D enemyDiedBulletRigid = enemyDiedBullet.GetComponent<Rigidbody2D>();
        float bulletSpeed = enemyDiedBullet.GetComponent<BulletController>().GetBulletSpeed();
        enemyDiedBulletRigid.AddForce(playerToMouseVector * bulletSpeed);


        // 삼킴 상태 해제
        isSwallowed = false;
        // 애니메이션 설정(삼킴 상태 해제)
        anim.SetBool("isSwallowed", false);
        return;
    }
}
