using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public GameObject bulletPrefab;
    private BulletController bulletControllerScr;
    private float generateTime;
    // Start is called before the first frame update
    void Start()
    {
        bulletControllerScr = bulletPrefab.GetComponent<BulletController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GenerateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        float bullletSpeed = bulletControllerScr.GetBulletSpeed();
        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-bullletSpeed, 0));
        yield return null;
    }
}
