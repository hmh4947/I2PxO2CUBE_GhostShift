using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject hitEffect;
    private PlayerShieldController playerShieldControllerScr;
    public GameObject playerBullet;
    private BulletController bulletControllerScr;
    public AudioClip parryingSfx;
    private new AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        playerShieldControllerScr = GetComponentInParent<PlayerShieldController>();
        audio = GetComponentInParent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Bullet")
        {
            playerShieldControllerScr.setDefended(true);
            audio.clip = parryingSfx;
            audio.Play();
            GameObject hitflash = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(hitflash, 0.2f);
            CameraShake.Instance.OnShakeCamera();
            if (playerShieldControllerScr.IsParrying == true)
            {
                Debug.Log("패링 진입");
                GameObject bullet = Instantiate(playerBullet, transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-collider.GetComponent<Rigidbody2D>().velocity.x, 0));
            }
            Destroy(collider.gameObject);

        }
    }
}
