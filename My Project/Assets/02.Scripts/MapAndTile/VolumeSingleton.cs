using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolumeSingleton : MonoBehaviour
{
    
    private GameObject globalVolumeObj;

    private static VolumeSingleton instance=null;
    
    public static VolumeSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<VolumeSingleton>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(VolumeSingleton).Name);
                    instance = singletonObject.AddComponent<VolumeSingleton>();
                }
            }
            return instance;
        }
    }
    
    void Awake()
     {

        if (instance == null )
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // 현재 게임 오브젝트를 인스턴스로 설정하고 파괴되지 않도록 설정
        else
        {
            Destroy(this.gameObject);
        }
    }
  

}
