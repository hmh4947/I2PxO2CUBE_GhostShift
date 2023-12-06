using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private new AudioSource audio;

    public AudioClip clickedAudio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        audio.PlayOneShot(clickedAudio);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;     
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Option()
    {
        audio.PlayOneShot(clickedAudio);
        Debug.Log("옵션창 출력");
        pauseMenuUI.SetActive(true);
    }
    public void StageExit()
    {
        audio.PlayOneShot(clickedAudio);
        SceneManagerEx.Instance.LoadScene(SceneManagerEx.Scenes.Title);
        Resume();
    }
    public void QuitGame()
    {
        audio.PlayOneShot(clickedAudio);
        Application.Quit();

    }
    

}
