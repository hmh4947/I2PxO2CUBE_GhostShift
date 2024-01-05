using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeTitle : MonoBehaviour
{
    public CanvasGroup image;
    private float startTime;
  
    // Update is called once per frame
    private void Start()
    {


    }
    void Update()
    {
        if (Time.time >= 2.0f)
        {

            if (image.alpha < 1)
            {

                image.alpha += 0.5f * Time.time;

            }


        }
        if (Time.time >= 2.5f)
        {
            SceneManager.LoadScene("Title");
        }
    }
}
