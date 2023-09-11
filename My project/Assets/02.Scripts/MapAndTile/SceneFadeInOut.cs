using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeInOut : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float frameTime = 1f;

     public static SceneFadeInOut instance = null;
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
         if (instance!= null)
         {
             DestroyImmediate(this.gameObject);
             return;

         }
         instance = this;
         DontDestroyOnLoad(gameObject);
     }
     
    public void Fade()
    {
        StartCoroutine(FadeFlow());

    }
    
  IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a<1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Map2");
        yield return new WaitForSeconds(0.5f);
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        yield return null;
    }
   
   
}
