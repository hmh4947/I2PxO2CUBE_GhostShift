using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class PlayerGogglesController : MonoBehaviour, IPlayerController
{
    public float MaxSpeed { get; set; }
    public float jumpPower { get; set; }
    public bool isInNVDModes { get; set; } // NVD: Night Vision Device

    public Rigidbody2D rigid { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }
    public Animator anim { get; set; }
    public CapsuleCollider2D playerCollider { get; set; }

    public GameObject playerGhost;
    public GameObject tileMap;
    public GameObject background;
    private Material originalBackground;
    public Material NVDBackground;

    PlayerGhostController playerGhostControllerScr;
    private Tilemap tileMapSpr;

    // Start is called before the first frame update
    private void Start()
    {
        originalBackground = background.GetComponent<MeshRenderer>().material;
        Init();
        SetBasicComponent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            float curAniTime = anim.GetCurrentAnimatorStateInfo(0).length;
            Debug.Log(curAniTime);
        }

        //Jump
        Jump();

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Health healthScr = gameObject.GetComponentInParent<Health>();
            {
                healthScr.Damaged(1);
            }
        }

    }

    //기본 세팅
    public virtual void Init()
    {
        //Move Variable
        MaxSpeed = 7.5f;

        //Jump Variable
        jumpPower = 22f;

        isInNVDModes = false;

        playerGhostControllerScr = playerGhost.GetComponent<PlayerGhostController>();
        playerGhostControllerScr.isPossesing = true;

        tileMapSpr = tileMap.GetComponent<Tilemap>();
        ChangeColorToWhite();
    }

    //기본 세팅2
    public virtual void SetBasicComponent()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponentInChildren<CapsuleCollider2D>();
    }
    //중력
    public virtual void Gravity()
    {
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
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
    public virtual void Jump()
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
    public virtual void Move()
    {
        if (!isInNVDModes)
        {
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > MaxSpeed) // Right Max Speed
                rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < MaxSpeed * (-1)) // Left Max Speed
                rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);
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
        playerGhost.transform.position = this.transform.position;

        playerGhost.SetActive(true);
        playerGhostControllerScr.ChangePlayerToGhost();
        gameObject.SetActive(false);
    }


}
