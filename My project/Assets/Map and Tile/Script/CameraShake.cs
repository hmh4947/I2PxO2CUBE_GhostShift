using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
   
    private float shakeTime;    // ��鸲 ���� �ð�  
    private float shakeIntensity;   // ��鸲 ����  

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            OnShakeCamera(0.1f, 1f);
        }
    }

   public void OnShakeCamera(float shakeTime=1.0f, float shakeIntensity=1.0f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;
        StopCoroutine("ShakeByPosition");
        StartCoroutine("ShakeByPosition");
    }
    private IEnumerator ShakeByPosition()
    {
        Vector3 startPosition=transform.position;
        while(shakeTime > 0f) 
        {
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;
            shakeTime -= Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
    }
}