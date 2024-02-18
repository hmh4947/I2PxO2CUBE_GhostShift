using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
   
    private float shakeTime;    // 흔들림 지속 시간  
    private float shakeIntensity;   // 흔들림 세기  
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public Camera mainCamera;

    private static CameraShake instance;
    public static CameraShake Instance=>instance;

  /*  private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            OnShakeCamera(0.1f, 1f);
        }
    }*/
  public CameraShake()
    {
        instance = this;
    }
   public void OnShakeCamera(float shakeTime=0.5f, float shakeIntensity=0.3f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;
        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
        instance = this;
       
    }
    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition=transform.position;
        Vector3 originalPosition = startPosition;
        while (shakeTime > 0f) 
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
            Vector3 newPosition = originalPosition + shakeOffset;

            newPosition.x = Mathf.Clamp(newPosition.x, mainCamera.transform.position.x+minX, mainCamera.transform.position.x+ maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, mainCamera.transform.position.y+minY, mainCamera.transform.position.y+maxY);
            transform.position = newPosition;
            shakeTime -= Time.deltaTime;
            yield return null;
            
        }
        transform.position = startPosition;
    }
}