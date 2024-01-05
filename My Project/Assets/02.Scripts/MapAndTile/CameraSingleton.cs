using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingleton : MonoBehaviour
{
    private static CameraSingleton _instance=null;
    void Start()
    {
        if (_instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
   
}
