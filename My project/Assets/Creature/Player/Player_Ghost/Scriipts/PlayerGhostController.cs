using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGhostController : MonoBehaviour, IPlayerController
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private bool isDashing;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float dashDuration;
    [SerializeField]
    private bool isAbleDash;
    [SerializeField]
    private bool isPossesing;
    [SerializeField]
    private bool isSticking;
    [SerializeField]
    private float animSpeed;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public CapsuleCollider2D playerCollider;

    public GameObject playerShield;
    public GameObject playerGoggles;
    public GameObject hitEffect;
    private GameObject enemyObject;

    [SerializeField]
    private EnemyType enemyType;
    private Enemy enemyScr;

    private AudioSource audio;

    public AudioClip jumpSfx;
    public AudioClip dashSfx;
    public AudioClip attack1Sfx;
    public AudioClip attack2Sfx;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
        SetBasicComponent();

        enemyScr = GameObject.Find("Enemy").GetComponent<Enemy>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(anim);
            Debug.Log(rigid);
            Debug.Log($"Dashing: {isDashing}" );
            Debug.Log($"isSticking: {isSticking}");
            Debug.Log($"isAbleDash: { isAbleDash}");

        }

        //Jump
        Jump();

        // �Ͻ����� �޴� Ŭ���� �ÿ� �Ǵ°� ����
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �޶���� �����ϰ�� �޶�ٱ⸦ �����ϰ� �뽬
                if (isSticking)
                {
                    Destroy(enemyObject);
                    isSticking = false;
                    rigid.gravityScale = 8.0f;
                    StopCoroutine(StickTo());
                }
                // �������� ��� ���Ǹ� �����ϰ� �뽬
                else if (isPossesing)
                {
                    isPossesing = false;
                    Init();
                    StartCoroutine(Dash());
                }
                // �ƴҰ�� �׳� �뽬
                StartCoroutine(Dash());
            }
            // ���콺 ��Ŭ�� �̺�Ʈ
            if (Input.GetMouseButtonDown(1))
            {
                // �޶�ٱ�, ���� ���� �ƴ� ��� ����
                if (isSticking && !isPossesing)
                {
                    Destroy(enemyObject);
                    isSticking = false;
                    isPossesing = true;

                    ChangePlayer(enemyType);
                }
               
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
            if (isDashing)
            {
                if (collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemyObject = collider.gameObject;
                    enemy.Died();
;                   enemyType = enemy.EnemyType;
                    isSticking = true;
                    this.transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y + 0.5f, this.transform.position.z);
                    StopCoroutine(Dash());
                    StartCoroutine(StickTo());
                }
                
            }
            else
            {
                Health healthScr = gameObject.GetComponentInParent<Health>();
                {
                    healthScr.Damaged(1);
                }
            }
        }

    }

    //�⺻ ����
    public void Init()
    {
        this.gameObject.SetActive(true);
        playerShield.SetActive(false);

        //Move Variable
        maxSpeed = 14.0f;

        //Jump Variable
        jumpPower = 22.0f;

        //Dash Variable
        dashSpeed = 30.0f;
        dashDuration = 0.2f;
        isDashing = false;
        isAbleDash = true;


        //Possession Variable
        isPossesing = false;
        isSticking = false;

        //Audio Variable
        audio = GetComponent<AudioSource>();
    }

    //�⺻ ����2
    public void SetBasicComponent()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        playerCollider = GetComponentInChildren<CapsuleCollider2D>();
    }

    public void Gravity()
    {
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {
                    isAbleDash = true;
                    anim.SetBool("isJumping", false);
                }

            }
        }
    }
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            if (isDashing == false && isSticking == false && isAbleDash == true)
            {
                audio.PlayOneShot(jumpSfx);
                anim.speed = 1.0f;
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }

        }
    }

    public void Move()
    {

        //Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
        {
            anim.SetBool("isWalking", false);
            anim.speed = 1.0f;
        }

        else
        {
            anim.SetBool("isWalking", true);
            if (!isDashing)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animSpeed = Random.Range(0.2f, 1.5f);
                    anim.speed = animSpeed;
                }

            }
        }

        if (isDashing == false && isSticking == false)
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
    }

    //�뽬
    public IEnumerator Dash()
    {
        //�뽬�� ������ ���
        if (isAbleDash)
        {
            anim.speed = 1.0f;
            anim.SetBool("isJumping", false);
            anim.SetBool("isDashing", true);
            audio.PlayOneShot(dashSfx);
            isDashing = true;
            isAbleDash = false;
            isSticking = false;
            var originalGravityScale = rigid.gravityScale;
            rigid.gravityScale = 0f;

            Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;

            //�뽬�� �� ���콺 ��ġ�� ���� ȸ��
            if (playerToMouseVector.x > 0)
            {
                if (spriteRenderer.flipX == false)
                    spriteRenderer.flipX = true;
            }
            else
            {
                if (spriteRenderer.flipX == true)
                    spriteRenderer.flipX = false;
            }
            rigid.velocity = playerToMouseVector * dashSpeed;

            // �浹üũ
            yield return new WaitForSeconds(dashDuration);

            anim.SetBool("isJumping", true);
            anim.SetBool("isDashing", false);


            //������ �޶���� ������ ��� �ٷ� �ڷ�ƾ ����
            if (isSticking)
            {
                audio.Stop();
                yield break;
            }

            isDashing = false;
            rigid.gravityScale = originalGravityScale;
        }
    }

    

    //�޶�ٱ�
    public IEnumerator StickTo()
    {
        animSpeed = 1.0f;
        anim.SetBool("isSticking", true);
        isDashing = false;
        isAbleDash = true;
        rigid.gravityScale = 0f;
        rigid.velocity = new Vector2(0, 0);


        //�޶�ٱⰡ ����� ������ ���
        yield return new WaitUntil(() => isSticking == false);
        anim.SetBool("isSticking", false);
        GameObject hitflash = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(hitflash, 0.2f);
        rigid.gravityScale = 8.0f;


    }

    //���� ĳ���ͷ� ����
    public void ChangePlayer(EnemyType enemyType)
    {
        Init();
        rigid.gravityScale = 8.0f;

        switch(enemyType){
            case EnemyType.None:
                return;
            case EnemyType.Shield:
                playerShield.transform.position = this.transform.position;
                this.gameObject.SetActive(false);
                playerShield.SetActive(true);
                return;
            case EnemyType.Goggles:
                playerGoggles.transform.position = this.transform.position;
                this.gameObject.SetActive(false);
                playerGoggles.SetActive(true);
                return;

        }
        

    }

    public void ChangePlayerToGhost()
    {
        GameObject hitflash = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(hitflash, 0.2f);
        isPossesing = false;
        CameraShake.Instance.OnShakeCamera();
        StartCoroutine(Dash());
    }

    public bool IsPossesing() { return isPossesing; }
    public void setPossesing(bool isPossesing) { this.isPossesing = isPossesing; }
}
