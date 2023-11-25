using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public GameObject bulletPrefab;
    private float generateTime;
    // Start is called before the first frame update
    void Start()
    {
        generateTime = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        generateTime -= Time.deltaTime;
        if(generateTime <= 0)
        {
            StartCoroutine(GenerateBullet());
            generateTime = 3.0f;
        }
        
    }

    public IEnumerator GenerateBullet()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        yield return null;
    }
}
