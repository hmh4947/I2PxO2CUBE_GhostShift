using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType EnemyType;
    private Animator anim;

    private PlayerGhostController playerGhostControllerScr;

    [SerializeField]
    private bool isDied;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isDied = false;
        playerGhostControllerScr = GameObject.Find("PlayerGhost").GetComponent<PlayerGhostController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Died()
    {
        anim.SetBool("isDied", true);
        isDied = true;
    }

    public bool IsDied() { return this.isDied; }

}
