using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadSingleton : MonoBehaviour
{
    private static QuadSingleton instance = null;
    // Start is called before the first frame update
    void Start()
    {

        if (instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
