using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestroy : MonoBehaviour
{
    public static PlayerDestroy instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }

    void Strat()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;

        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
