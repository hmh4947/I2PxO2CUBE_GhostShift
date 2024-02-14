using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreviousMap_gate2 : MonoBehaviour
{
    public SceneManagerEx.Scenes scene;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
           
            GameObject.Find("fadeout").GetComponent<SceneFadeInOut>().ReturnMap2();


        }
    }
    // Start is called before the first frame update
}

