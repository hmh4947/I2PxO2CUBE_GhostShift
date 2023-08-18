using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgroud : MonoBehaviour
{
    private MeshRenderer render;

    public float speed;
    private float offset;

    
    // Start is called before the first frame update
    void Start()
    {
      
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyCheck();
    }
    void KeyCheck()
    {
        if(Input.GetKey("a"))
        {
            offset += Time.deltaTime * -0.1f;
            render.material.mainTextureOffset = new Vector2(offset, 0);
        }
        if(Input.GetKey("d"))
        {
            offset += Time.deltaTime * 0.1f;
            render.material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
