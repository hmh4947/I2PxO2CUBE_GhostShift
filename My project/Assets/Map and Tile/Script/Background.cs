using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private MeshRenderer render;

    public float speed;
    private float offset;

    public Transform playerObject;
    private Camera mainCamera;

    private Transform cameraTransform;

    public float stopXr; // 멈출 X 좌표
    public float stopXl;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        render = GetComponent<MeshRenderer>();

        cameraTransform = Camera.main.transform;
     
    }

    // Update is called once per frame
    void Update()
    {
        float currentCameraX = cameraTransform.position.x;
      
        if (currentCameraX <stopXr && currentCameraX > stopXl)
        {


            if (GameObject.Find("Player").GetComponent<PlayerMove>().isDashing == true)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(playerObject.position);
                    Vector2 mouseScreenPosition = Input.mousePosition;
                    Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;

                    //대쉬할 때 마우스 위치에 따라 회전
                    if (playerToMouseVector.x > 0)
                    {
                        offset += Time.deltaTime * 0.06f;
                        render.material.mainTextureOffset = new Vector2(offset, 0);

                    }
                    if ((playerToMouseVector.x < 0))
                    {
                        offset += Time.deltaTime * -0.06f;
                        render.material.mainTextureOffset = new Vector2(offset, 0);


                    }
                    else
                    {
                        offset += Time.deltaTime * 0f;
                        render.material.mainTextureOffset = new Vector2(offset, 0);

                    }

                }
            }
            KeyCheck();

        }
    
    }
    void KeyCheck()
    {
        float currentCameraX = cameraTransform.position.x;
        if (currentCameraX < stopXr&& currentCameraX > stopXl)
            {

            if (Input.GetKey("a"))
            {
                offset += Time.deltaTime * -0.03f;
                render.material.mainTextureOffset = new Vector2(offset, 0);
            }
            if (Input.GetKey("d"))
            {
                offset += Time.deltaTime * 0.03f;
                render.material.mainTextureOffset = new Vector2(offset, 0);
            }
        }


    }
}
