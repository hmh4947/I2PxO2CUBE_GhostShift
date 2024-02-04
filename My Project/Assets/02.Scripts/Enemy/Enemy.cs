using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType EnemyType;
    private Animator anim;
    private CapsuleCollider2D coll;
    private PlayerGhostController playerGhostControllerScr;
    private Rigidbody2D rigid;
    private float knockBackPower;

    [SerializeField]
    private bool isDied;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        isDied = false;
        playerGhostControllerScr = GameObject.Find("Player").GetComponent<PlayerGhostController>();
        rigid = GetComponent<Rigidbody2D>();
        knockBackPower = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Died()
    {
        anim.SetBool("isDied", true);
        isDied = true;
        coll.isTrigger = true;
    }

    public void KnockBack(Vector2 dir)
    {
        rigid.AddForce(knockBackPower * dir, ForceMode2D.Impulse);
    }
    public bool IsDied() { return this.isDied; }

}
