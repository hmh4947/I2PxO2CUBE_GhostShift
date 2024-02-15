using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextMap : MonoBehaviour
{
    public SceneManagerEx.Scenes scene;
  
    
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            Debug.Log("next스크립트 fadeflow 함수 실행");
            GameObject.Find("fadeout").GetComponent<SceneFadeInOut>().Fade();
           
            
        }
    }
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
