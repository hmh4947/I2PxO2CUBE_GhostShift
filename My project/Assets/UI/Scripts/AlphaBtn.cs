using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��ư�� �̹��� ��翡 �°� ũ�⸦ �����ϴ� Ŭ����
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
