using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType EnemyType;
    private Animator anim;
    private CapsuleCollider2D coll;
    private PlayerGhostController playerGhostControllerScr;

    [SerializeField]
    private bool isDied;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        isDied = false;
        playerGhostControllerScr = GameObject.Find("Player").GetComponent<PlayerGhostController>();
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

    public bool IsDied() { return this.isDied; }

}
