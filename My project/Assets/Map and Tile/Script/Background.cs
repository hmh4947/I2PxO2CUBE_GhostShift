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
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        render = GetComponent<MeshRenderer>();
       
    }

    // Update is called once per frame
   void Update()
    {
         
        if (Input.GetMouseButton(0))
        {
            Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(playerObject.position);
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 playerToMouseVector = (mouseScreenPosition - playerScreenPosition).normalized;

            //�뽬�� �� ���콺 ��ġ�� ���� ȸ��
            if (playerToMouseVector.x > 0)
            {
                offset += Time.deltaTime * 0.5f;
                render.material.mainTextureOffset = new Vector2(offset, 0);
 
            }
            if((playerToMouseVector.x < 0))
            {
                offset += Time.deltaTime * -0.5f;
                render.material.mainTextureOffset = new Vector2(offset, 0);


            }
            else
            {
                offset += Time.deltaTime *0f;
                render.material.mainTextureOffset = new Vector2(offset, 0);
                
            }
     
        }
       KeyCheck();
    }
    void KeyCheck()
    {
       

         if (Input.GetKey("a"))
         {
             offset += Time.deltaTime * -0.1f;
             render.material.mainTextureOffset = new Vector2(offset, 0);
         }
         if (Input.GetKey("d"))
         {
             offset += Time.deltaTime * 0.1f;
             render.material.mainTextureOffset = new Vector2(offset, 0);
         }
         

    }
}
