using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class Map2SceneManager : MonoBehaviour
{

    private GameObject globalvolumeObj;
    public GameObject objectToDisable;
    void Update()
    {
        SceneState();
    }
    void SceneState()
     {
       
        string currentSceneName = SceneManager.GetActiveScene().name;
       
         if (globalvolumeObj! == null)
        {
             // 현재 씬이 "Map2"이고 Player 오브젝트가 있는 경우에만 작업을 수행
             if (currentSceneName == "Map2")
             {
                objectToDisable.SetActive(true);
                //Debug.Log("현재 씬 이름: " + currentSceneName);
 
             }
             else
            {
              
                objectToDisable.SetActive(false);
                //Debug.Log("현재 씬 이름: " + currentSceneName);
              
            }
         }
     }
    
    

}
