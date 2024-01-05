using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject blockedBulletPrefab;
    private GameObject hitEffect;

    // 醚舅 加档
    public float bulletSpeed;
    // Start is called before the first frame update
    public void Start()
    {
        hitEffect = Resources.Load<GameObject>("Effects/FX_Hit");
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetBulletSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }

    public float GetBulletSpeed()
    {
        return this.bulletSpeed;
    }

    // 面倒 贸府
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.tag == "PlayerBullet")
        {
            if(collision.tag == "Enemy" )
            {
                GameObject effect = Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
                Destroy(effect, 0.2f);
            }

        }
    }


    public void generateBlockedBullet()
    {
        Instantiate(blockedBulletPrefab, transform.position, transform.rotation);
        blockedBulletPrefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletSpeed, 0.0f));
        Destroy(this.gameObject);
    }
}
