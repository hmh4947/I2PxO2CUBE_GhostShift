using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject hitEffect;
    private PlayerShieldController playerShieldControllerScr;

    // Start is called before the first frame update
    void Start()
    {
        playerShieldControllerScr = gameObject.GetComponentInParent<PlayerShieldController>();

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            playerShieldControllerScr.defended = true;
            GameObject hitflash = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(hitflash, 0.2f);
            CameraShake.Instance.OnShakeCamera();
            Destroy(collision.gameObject);
        }
    }
}
