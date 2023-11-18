using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject blockedBulletPrefab;
    public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector2(bulletSpeed, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 5.0f);
    }

    public void generateBlockedBullet()
    {
        Instantiate(blockedBulletPrefab, transform.position, transform.rotation);
        blockedBulletPrefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(-bulletSpeed, 0.0f));
        Destroy(this.gameObject);
    }
}