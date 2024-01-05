using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleBtn : MonoBehaviour
{
    private new AudioSource audio;
    public AudioClip clickedAudio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.playOnAwake = false;
    }
    public void StartBtn()
    {
        SceneManager.LoadScene("Tutorial");
        audio.PlayOneShot(clickedAudio);
    }
    public void OptionBtn()
    {
        //SceneManager.LoadScene("BasicMenu");
        audio.PlayOneShot(clickedAudio);

    }
    public void ExitBtn()
    {
        audio.PlayOneShot(clickedAudio);
        Application.Quit();
    }
}
