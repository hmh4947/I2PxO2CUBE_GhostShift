using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShieldController : MonoBehaviour, IPlayerController
{
    public float parryingDuration;
    public float maxSpeed;

    public bool isParrying;
    [SerializeField]
    private bool isDefending;
    [SerializeField]
    private bool defended;
    // ���� �ݶ��̴� ���� ������
    private Vector2 shieldPosition;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private CapsuleCollider2D playerCollider;
    private Transform tr;
    private Health healthScr;

    public GameObject playerGhost;
    public GameObject shield;
    private PlayerGhostController playerGhostControllerScr;
    private new AudioSource audio;

    public AudioClip swingSfx;
    // Start is called before the first frame update
    private void Start()
    {
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

        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �÷��̾� �ٲٸ鼭 �뽬
                ChangePlayer();
            }
            // ���콺 ��Ŭ�� �̺�Ʈ
            if (Input.GetMouseButtonDown(1))
            {
                if (!isParrying)
                {
                    StartCoroutine(Parrying());
                }
                    
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(Defending());
            }
            if (isDefending)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    isDefending = false;
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            healthScr.Damaged(1);
        }

        if(collider.gameObject.tag == "Bullet")
        {
            if (!defended)
            {
                healthScr.Damaged(1);
                Destroy(collider.gameObject);
            }
        }

    }

    //�⺻ ����
    public virtual void Init()
    {
        //Move Variable
        maxSpeed = 7.5f;

        isParrying = false;
        parryingDuration = 0.25f;

        isDefending = false;
        shield.SetActive(false);

        defended = false;

        shieldPosition = new Vector3(0.4263f, 0, 0);
}

    // �޸� �Ҹ� ���̱� ���� ĳ�� ����
    public virtual void SetCashComponent()
    {
        // �÷��̾� ������Ʈ ĳ�� ó��
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponentInChildren<CapsuleCollider2D>();
        tr = GetComponent<Transform>();

        //Script ĳ�� ó��
        healthScr = gameObject.GetComponentInParent<Health>();
        playerGhostControllerScr = playerGhost.GetComponent<PlayerGhostController>();
        audio = GetComponent<AudioSource>();
    }

    //�߷�
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
    //�̵�
    public virtual void Move()
    {
        if (!isParrying && !isDefending)
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
            if(Input.GetAxisRaw("Horizontal") == 1)
            {
                spriteRenderer.flipX = true;
                shield.transform.localPosition = shieldPosition;
            }
            else
            {
                spriteRenderer.flipX = false;
                shield.transform.localPosition = shieldPosition * -1;
            }
        }

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }
    // �и�
    public IEnumerator Parrying()
    {
        anim.SetBool("isParrying", true);
        isParrying = true;
        audio.clip = swingSfx;
        audio.Play();
        shield.SetActive(true);

        yield return new WaitForSeconds(parryingDuration);

        isParrying = false;
        anim.SetBool("isParrying", false);
        shield.SetActive(false);
        if (defended)
            defended = false;
    }
    // ��� ���
    public IEnumerator Defending()
    {
        anim.SetBool("isDefending", true);
        isDefending = true;
        shield.SetActive(true);

        yield return new WaitUntil(() => isDefending == false);
        anim.SetBool("isDefending", false);
        isDefending = false;


    }
    //���� ĳ���ͷ� ����
    public void ChangePlayer()
    {
        Init();
        rigid.gravityScale = 8.0f;
        playerGhost.transform.position = tr.position;

        playerGhost.SetActive(true);
        playerGhostControllerScr.ChangePlayerToGhost();
        gameObject.SetActive(false);
    }

    public void setDefended(bool defended)
    {
        this.defended = defended;
    }
}
