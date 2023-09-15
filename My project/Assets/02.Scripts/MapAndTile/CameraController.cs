using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public GameObject targetObject;
    public GameObject targetObject1;//Ȱ��ȭ ���� �˻�
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
               //�ڽ� ������Ʈ�� ã��
                Transform playerGhoset = playerObject.transform.Find("PlayerGhoset");

                if (playerGhoset != null)
                {
                    // ã�� "PlayerGhoset"�� ���ο� target���� ����
                    target = playerGhoset;
                }
            }
        }
        if (targetObject1.activeSelf)
        {
            GameObject playerObject = GameObject.Find("Player");

            if (playerObject != null)
            {
                //�ڽ� ������Ʈ�� ã��
                Transform playerShield = playerObject.transform.Find("PlayerShield");

                if (playerShield != null)
                {
                    // ã�� "PlayerShield"�� ���ο� target���� ����
                    target = playerShield;
                }
            }
        }
        if (targetObject2.activeSelf)
        {    
            GameObject playerObject = GameObject.Find("Player");

            if (playerObject != null)
            {
                //�ڽ� ������Ʈ�� ã��
                Transform playerGoggles = playerObject.transform.Find("PlayerGoggles");

                if (playerGoggles != null)
                {
                    // ã�� "PlayerGoggles"�� ���ο� target���� ����
                    target = playerGoggles;
                }
            }
        }
    }
   
}