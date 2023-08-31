using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector2(-150.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 5.0f);
    }


}
