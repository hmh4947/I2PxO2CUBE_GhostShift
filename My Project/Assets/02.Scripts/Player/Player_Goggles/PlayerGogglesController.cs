using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class PlayerGogglesController : PlayerController
{
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private bool isInNVDModes; // NVD: Night Vision Device

    private GameObject tileMap;
    private GameObject background;


    private Material originalBackground;
    private Material NVDBackground;

    PlayerGhostController playerGhostControllerScr;
    private Tilemap tileMapSpr;

    public bool IsInNVDModes
    {
        get
        {
            return isInNVDModes;
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        SetScrCash();
        tileMap = GameObject.Find("Tilemap");
        background = GameObject.Find("Quad");
        originalBackground = background.GetComponent<MeshRenderer>().material;
        NVDBackground = Resources.Load<Material>("Materials/NVDMaterial");
        SetCashComponent();
        Init();
    }

    private void OnEnable()
    {
        SetScrCash();
        tileMap = GameObject.Find("Tilemap");
        background = GameObject.Find("Quad");
        originalBackground = background.GetComponent<MeshRenderer>().material;
        SetCashComponent();
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float curAniTime = anim.GetCurrentAnimatorStateInfo(0).length;
            Debug.Log(curAniTime);
        }

        // 일시정지 메뉴 클릭할 시에 되는걸 방지
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 플레이어 바꾸면서 대쉬
                ChangePlayer();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(NightVisionModes());
            }
            if (isInNVDModes)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    isInNVDModes = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move By Key Control(Move Speed)
        Move();

        //Landing Platform
        Gravity();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!enabled) return;
        if (collider.gameObject.tag == "Enemy")
        {
            Health healthScr = gameObject.GetComponentInParent<Health>();
            {
                healthScr.Damaged(1);
            }
        }

    }

    //기본 세팅
    public  override void Init()
    {
        //Move Variable
        maxSpeed = 7.5f;

        //Jump Variable
        jumpPower = 22f;

        isInNVDModes = false;

        ChangeColorToWhite();
    }

    //기본 세팅2
    public  override void SetCashComponent()
    {
        base.SetCashComponent();

        playerGhostControllerScr = GetComponent<PlayerGhostController>();
        tileMapSpr = tileMap.GetComponent<Tilemap>();
        tr = GetComponent<Transform>();
    }
    //중력
    public  override void Gravity()
    {
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            /*if(Physics.CheckBox(, 0.2f))*/
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {
                    anim.SetBool("isJumping", false);
                }

            }
        }
    }
    //점프
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            if (!isInNVDModes)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }

        }
    }
    //이동
    public override void Move()
    {
        if (!isInNVDModes)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed) // Right Max Speed
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    public void ChangeColorToGreen()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.green;
        tileMapSpr.color = Color.green;
        background.GetComponent<MeshRenderer>().material = NVDBackground;
    } 

    public void ChangeColorToWhite()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        tileMapSpr.color = Color.white;
        background.GetComponent<MeshRenderer>().material = originalBackground;
    }
    public IEnumerator NightVisionModes()
    {
        anim.SetBool("isInNVDModes", true);
        isInNVDModes = true;

        ChangeColorToGreen();
        yield return new WaitUntil(()=> isInNVDModes == false);

        anim.SetBool("isInNVDModes", false);
        ChangeColorToWhite();
    }


    //유령 캐릭터로 변경
    public void ChangePlayer()
    {
        Init();
        rigid.gravityScale = 8.0f;
        playerScr.ChangePlayer(PlayerType.PLAYERGHOST);
    }


}
