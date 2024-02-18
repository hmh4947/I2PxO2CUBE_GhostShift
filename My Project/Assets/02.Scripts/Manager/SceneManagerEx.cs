using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerEx : MonoBehaviour
{
    private static SceneManagerEx instance;
    private Vector2 playerBasePosition;
    private GameObject player;
    private Player playerScr;
    private PlayerGogglesController playerGogglesControllerScr;
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
        PlayerCleanerTutorial,
        Stage1_Map1,
        Stage1_Map2,
        Stage1_Map3,
        Stage1_Map4,
        Stage1_Map5,
        Stage1_Map6,
        Stage1_Map7,
        Stage1_Map8
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
        playerGogglesControllerScr = player.GetComponent<PlayerGogglesController>();
        playerBasePosition = new Vector2(-18, 0);
    }

    public void LoadScene(Scenes scenename)
    {
        switch (scenename)
        {
            case Scenes.Title:
                SceneManager.LoadScene("Title");
                foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
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
            case Scenes.Stage1_Map1:
                SceneManager.LoadScene("Stage1_Map1");
                break;
            case Scenes.Stage1_Map2:
                SceneManager.LoadScene("Stage1_Map2");
                break;
            case Scenes.Stage1_Map3:
                SceneManager.LoadScene("Stage1_Map3");
                break;
            case Scenes.Stage1_Map4:
                SceneManager.LoadScene("Stage1_Map4");
                break;
            case Scenes.Stage1_Map5:
                SceneManager.LoadScene("Stage1_Map5");
                break;
            case Scenes.Stage1_Map6:
                SceneManager.LoadScene("Stage1_Map6");
                break;
            case Scenes.Stage1_Map7:
                SceneManager.LoadScene("Stage1_Map7");
                break;
            case Scenes.Stage1_Map8:
                SceneManager.LoadScene("Stage1_Map8");
                break;
            default:
                Debug.LogError("´Ù¸¥ ½ÅÀÌ µé¾î¿È");
                break;
        }
    }

    public void Update()
    {
        if (playerGogglesControllerScr.IsInNVDModes)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SetPlayerPositionAndCondition(playerBasePosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadScene(Scenes.Tutorial);
            SetPlayerPositionAndCondition(playerBasePosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadScene(Scenes.PlayerShieldTutorial);
            SetPlayerPositionAndCondition(playerBasePosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadScene(Scenes.PlayerGogglesTutorial);
            SetPlayerPositionAndCondition(playerBasePosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadScene(Scenes.PlayerCleanerTutorial);
            SetPlayerPositionAndCondition(playerBasePosition);
        }
    }

    public void SetPlayerPositionAndCondition(Vector2 pos)
    {
        player.transform.position = pos;
        playerScr.ChangePlayer(PlayerType.PLAYERGHOST);
    }
    
}
