using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleBtn : MonoBehaviour
{
   
    public void StartBtn()
    {
        SceneManager.LoadScene("Map1khd");
    }
    public void OptionBtn()
    {

    }
    public void ExitBtn()
    {
        Application.Quit();
    }
}
