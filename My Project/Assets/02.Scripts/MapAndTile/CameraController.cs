using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public GameObject targetObject;
    public GameObject targetObject1;//Ȱ��ȭ ���� �˻�
    public GameObject targetObject2;

    public GameObject quadScale;

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
        quadScale = GameObject.Find("Quad");

        //Ʈ������ ������Ʈ ��������
        Transform quadScaleTransform = quadScale.transform;
        float quadeScale_x = quadScaleTransform.localScale.x;
        float quadeScale_y = quadScaleTransform.localScale.y;

       // Debug.Log("X ������ ��: " + quadeScale_x);
       // Debug.Log("Y ������ ��: " + quadeScale_y);
        if (quadeScale_x == 32 && quadeScale_y == 18)
        {
            limitMinX = -19f;
            limitMaxX = 13f;
            limitMinY = -8f;
            limitMaxY = 10f;
        }
        if (quadeScale_x==42&& quadeScale_y == 24)
        {
            limitMinX = -19f;
            limitMaxX = 23f;
            limitMinY = -8f;
            limitMaxY = 16f;
        }
        if (quadeScale_x == 60 && quadeScale_y == 21)
        {
            limitMinX = -19f;
            limitMaxX =41f;
            limitMinY = -8f;
            limitMaxY = 13f;
        }
        if (quadeScale_x == 44 && quadeScale_y == 18)
        {

            limitMinX = -19f;
            limitMaxX = 25f;
            limitMinY = -8f;
            limitMaxY = 10f;
        }
        if (quadeScale_x ==42 && quadeScale_y ==18)
        {
            limitMinX = -19f;
            limitMaxX = 23f;
            limitMinY = -8f;
            limitMaxY = 10f;
        }
        if (targetObject.activeSelf)
        {
            GameObject playerObject = GameObject.Find("Player");

            if (playerObject != null)
            {
               //�ڽ� ������Ʈ�� ã��
                Transform playerGhost = playerObject.transform.Find("PlayerGhost");

                if (playerGhost != null)
                {
                    // ã�� "PlayerGhost"�� ���ο� target���� ����
                    target = playerGhost;
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