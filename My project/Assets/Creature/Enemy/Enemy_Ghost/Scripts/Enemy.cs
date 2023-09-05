using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public EnemyType EnemyType;

    public Sprite diedSprite;
    private PlayerGhostController playerGhostControllerScr;
    // Start is called before the first frame update
    void Start()
    {
        playerGhostControllerScr = GameObject.Find("PlayerGhost").GetComponent<PlayerGhostController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerGhostControllerScr.isSticking)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = diedSprite;
            }
        }
    }

    public void Died()
    {
        Destroy(gameObject);
    }
}
