using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    GameObject SplashObj;               //�ǳڿ�����Ʈ
    Image image;
    private bool checkbool = false;     //���� ���� ���� ����//�ǳ� �̹���
                                        // Start is called before the first frame update
    void Awake()

    {

        SplashObj = this.gameObject;                         //��ũ��Ʈ ������ ������Ʈ

        image = SplashObj.GetComponent<Image>();    //�ǳڿ�����Ʈ�� �̹��� ����

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("MainSplash");                        //�ڷ�ƾ    //�ǳ� ���� ����
        Debug.Log("1");
        StartCoroutine("MainSplash2");
        Debug.Log("2");
        if (checkbool)                                            //���� checkbool �� ���̸�

        {

            Destroy(this.gameObject);                        //�ǳ� �ı�, ����

        }
    }
    IEnumerator MainSplash()
    {
        Debug.Log("�ڷ�ƾ1");

        Color color = image.color;                            //color �� �ǳ� �̹��� ����



        for (int i = 100; i >= 0; i--)                            //for�� 100�� �ݺ� 0���� ���� �� ����

        {

            color.a -= Time.deltaTime * 0.01f;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.01



            image.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����



            if (image.color.a <= 0)                        //���� �ǳ� �̹��� ���� ���� 0���� ������

            {

                checkbool = true;                              //checkbool �� 

            }

        }

        yield return null;                                        //�ڷ�ƾ ����

    }
    IEnumerator MainSplash2()
    {
        Debug.Log("�ڷ�ƾ2");

        Color color = image.color;                            //color �� �ǳ� �̹��� ����



        for (int j = 0; j <= 100; j++)                            //for�� 100�� �ݺ� 0���� ���� �� ����

        {

            color.a += Time.deltaTime * 0.01f;               //�̹��� ���� ���� Ÿ�� ��Ÿ �� * 0.01



            image.color = color;                                //�ǳ� �̹��� �÷��� �ٲ� ���İ� ����



            if (image.color.a >= 0)                        //���� �ǳ� �̹��� ���� ���� 0���� ������

            {

                checkbool = true;                              //checkbool �� 

            }

        }

        yield return null;                                        //�ڷ�ƾ ����

    }
}
