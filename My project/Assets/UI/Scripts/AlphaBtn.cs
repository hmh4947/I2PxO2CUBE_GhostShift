using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//버튼을 이미지 모양에 맞게 크기를 조정하는 클래스
public class AlphaBtn : MonoBehaviour
{
    public float AlphaThreshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
