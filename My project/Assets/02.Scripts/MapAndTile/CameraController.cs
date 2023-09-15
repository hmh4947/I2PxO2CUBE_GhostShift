using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public GameObject targetObject;
    public GameObject targetObject1;//활성화 상태 검사
    public GameObject targetObject2;

    public float smoothSpeed = 3;
    public Vector2 offset;
    public float limitMinX, limitMaxX, limitMinY, limitMaxY;
    float cameraHalfWidth, cameraHalfHeight;

    private void Start()
    {
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        cameraHalfHeight = Camera.main.orthographicSize;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
            Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
            -10);                                                                                                  // Z
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
    private void Update()
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
                    // 찾은 "PlayerGhost"를 새로운 target으로 설정
                    target = playerGhost;
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
                    // 찾은 "PlayerShield"를 새로운 target으로 설정
                    target = playerShield;
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
                    // 찾은 "PlayerGoggles"를 새로운 target으로 설정
                    target = playerGoggles;
                }
            }
        }
    }
   
}