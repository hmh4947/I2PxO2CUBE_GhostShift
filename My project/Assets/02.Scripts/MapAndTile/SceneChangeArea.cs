using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeArea : MonoBehaviour
{
    public SceneManagerEx.Scenes scene;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            Debug.Log("Ãæµ¹");

            //GameObject.Find("Canvas").GetComponent<SceneFadeInOut>().Fade();
            SceneManagerEx.Instance.LoadScene(scene);
            SceneManagerEx.Instance.SetPlayerPositionAndCondition(new Vector2(-18, 0));
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
