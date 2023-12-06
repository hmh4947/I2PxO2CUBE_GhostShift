using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerEx : MonoBehaviour
{
    private static SceneManagerEx instance;
    private GameObject player;
    private Player playerScr;
    public static SceneManagerEx Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public enum Scenes
    {
        Title,
        Tutorial,
        PlayerShieldTutorial,
        PlayerGogglesTutorial,
        PlayerCleanerTutorial
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        player = GameObject.Find("Player");
        playerScr = player.GetComponent<Player>();
    }

    public void LoadScene(Scenes scenename)
    {
        switch (scenename)
        {
            case Scenes.Title:
                SceneManager.LoadScene("Title");
                foreach(GameObject o in Object.FindObjectsOfType<GameObject>())
                {
                    Destroy(o);
                }
                break;
            case Scenes.Tutorial:
                SceneManager.LoadScene("Tutorial");
                break;
            case Scenes.PlayerShieldTutorial:
                SceneManager.LoadScene("PlayerShieldTutorial");
                break;
            case Scenes.PlayerGogglesTutorial:
                SceneManager.LoadScene("PlayerGogglesTutorial");
                break;
            case Scenes.PlayerCleanerTutorial:
                SceneManager.LoadScene("PlayerCleanerTutorial");
                break;
            default:
                Debug.LogError("다른 신이 들어옴");
                break;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SetPlayerPositionAndCondition(new Vector2(-18, 0));
        }        
    }

    public void SetPlayerPositionAndCondition(Vector2 pos)
    {
        player.transform.position = pos;
        playerScr.ChangePlayer(PlayerType.PLAYERGHOST);
    }
    
}
