using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private MeshRenderer render;

    public float speed;
    private float offset;

    private Rigidbody2D playerRigid;
    private Player playerScr;
    private bool isChecked;

    private Camera mainCamera;
    private Transform cameraTransform;

    public float stopXr; // ∏ÿ√‚ X ¡¬«•
    public float stopXl;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        render = GetComponent<MeshRenderer>();

        playerRigid = GameObject.Find("Player").GetComponentInChildren<Rigidbody2D>();
        playerScr = GameObject.Find("Player").GetComponent<Player>();
        cameraTransform = Camera.main.transform;
     
    }

    // Update is called once per frame
    void Update()
    {
        float currentCameraX = cameraTransform.position.x;
      
        if (currentCameraX <stopXr && currentCameraX > stopXl)
        {
            if (playerScr.IsPossesing) 
            {
                if(isChecked == false)
                {
                    playerRigid = GameObject.Find("Player").GetComponentInChildren<Rigidbody2D>();
                    isChecked = true;
                }
            }
            else
            {
                if (isChecked)
                {
                    playerRigid = GameObject.Find("Player").GetComponentInChildren<Rigidbody2D>();
                    isChecked = false;
                }
            }

            MoveCamera();
        }
    
    }
    void MoveCamera()
    {
        if (playerRigid.velocity.x > 0.5)
        {
            offset += Time.deltaTime * 0.03f;
            render.material.mainTextureOffset = new Vector2(offset, 0);

        }
        else if (playerRigid.velocity.x < -0.5)
        {
            offset += Time.deltaTime * -0.03f;
            render.material.mainTextureOffset = new Vector2(offset, 0);
        }
        else
        {
            offset += Time.deltaTime * 0f;
            render.material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
