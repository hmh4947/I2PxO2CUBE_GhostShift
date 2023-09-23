using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class VolumeMove : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public GameObject targetObject;
    public GameObject targetObject1;//활성화 상태 검사
    public GameObject targetObject2;
    public GameObject volumeObj;
    
    Volume volume;
    Vignette vignette;

  
    private void Start()
    {
       
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out vignette))
        {
            // 시작할 때 초기 Center 값을 저장
            vignette.center.value = new Vector2(0.5f, 0.5f);
        }

        
    }

    private void Update()
    {
      //  string currentSceneName = SceneManager.GetActiveScene().name;
        VolumeTarget();
        /* if (currentSceneName == "Map2")
         {
             GameObject playerObject = GameObject.FindWithTag("Player");
             if (playerObject != null)
             {
                // Debug.Log(SceneManager.GetActiveScene().name);
                 volumeObj.SetActive(true);
                 VolumeTarget();
             }


         }
         else
         {

             Debug.Log(SceneManager.GetActiveScene().name);
             volumeObj.SetActive(false);

         }*/
    }

    public void VolumeTarget()
    {
        if (targetObject.activeSelf)
        {
            GameObject playerObject = GameObject.Find("Player");

            if (playerObject != null)
            {
                //자식 오브젝트를 찾음
                Transform playerGhost = playerObject.transform.Find("PlayerGhost");

                if (playerGhost != null)
                {
                    // 찾은 "PlayerGhost"를 새로운 player으로 설정
                    player = playerGhost;
                }
            }
        }
        if (targetObject1.activeSelf)
        {
            GameObject playerObject = GameObject.Find("Player");

            if (playerObject != null)
            {
                //자식 오브젝트를 찾음
                Transform playerShield = playerObject.transform.Find("PlayerShield");

                if (playerShield != null)
                {
                    // 찾은 "PlayerShield"를 새로운 player으로 설정
                    player = playerShield;
                }
            }
        }
        if (targetObject2.activeSelf)
        {
            GameObject playerObject = GameObject.Find("Player");

            if (playerObject != null)
            {
                //자식 오브젝트를 찾음
                Transform playerGoggles = playerObject.transform.Find("PlayerGoggles");

                if (playerGoggles != null)
                {
                    // 찾은 "PlayerGoggles"를 새로운 player으로 설정
                    player = playerGoggles;
                }
            }
        }


        if (player != null && vignette != null)
        {
            // 플레이어 위치에 따라 Vignette의 Center 값을 업데이트
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(player.position);
            vignette.center.value = new Vector2(viewportPos.x, viewportPos.y);
        }
    }
}
