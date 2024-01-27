using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
public class Guide : MonoBehaviour
{
   // public GameObject ghost;
    public GameObject goggles_guide;
    public GameObject shield_guide;
    public GameObject cleaner_guide;

    public Image gogglesImage;
    public Image shieldImage;
    public Image cleanerImage;

    float time = 0f;
    float frameTime = 1f;

    void Start()
    {
     //   ghost.SetActive(false);
        goggles_guide.SetActive(false);
        shield_guide.SetActive(false);
        cleaner_guide.SetActive(false);
       

      /*  //image ������Ʈ ��������
        Image gogglesImage=GetComponent<Image>();
        Image shieldImage = GetComponent<Image>();
        Image cleanerImage = GetComponent<Image>();

        */

    }

    // Update is called once per frame
    void Update()
    {
        Transform parentTransform = transform;
        Transform GhostObj = parentTransform.GetChild(0); // Player�� �ڽ� ������Ʈ �� Ghost
        Transform ShieldObj = parentTransform.GetChild(1); // Player�� �ڽ� ������Ʈ �� Shield
        Transform GogglesObj = parentTransform.GetChild(2); // Player�� �ڽ� ������Ʈ �� Goggle
        Transform CleanerObj = parentTransform.GetChild(5); // Player�� �ڽ� ������Ʈ �� Cleaner
                                                          

        Color alpha_goggles = gogglesImage.color;
        Color alpha_shield = shieldImage.color;
        Color alpha_cleaner = cleanerImage.color;

        
        if (GhostObj != null && GhostObj.gameObject.activeSelf == true)//GhostObj Ȱ��ȭ ���� 
        {
          
            Debug.Log("��Ʈ");
            goggles_guide.SetActive(false);
            shield_guide.SetActive(false);
            cleaner_guide.SetActive(false);
            StopCoroutine(WaitForTenSecond());
            shieldImage.color = new Color(255, 255, 255, 1); //���İ� �ʱ�ȭ
                                                           

            // �ڽ� ��ü�� Ȱ��ȭ�Ǿ� ����
            //  Debug.Log("��Ʈ");

        }
        if (ShieldObj != null && ShieldObj.gameObject.activeSelf == true)
        {

            // ���̵� ui
            shield_guide.SetActive(true);


            //10�� ��ٸ���
            StartCoroutine(WaitForTenSecond());
            //StopCoroutine(WaitForTenSecond());
            
            Debug.Log("����");
        }

        if (GogglesObj != null && GogglesObj.gameObject.activeSelf == true)
        {
            // �ڽ� ��ü�� Ȱ��ȭ�Ǿ� ����

            goggles_guide.SetActive(true);
            Debug.Log("���");
            StartCoroutine(WaitForTenSecond());

        }


        if (CleanerObj != null && CleanerObj.gameObject.activeSelf == true)
        {
            // �ڽ� ��ü�� Ȱ��ȭ�Ǿ� ����
            cleaner_guide.SetActive(true);

            Debug.Log("û�ұ�");

            StartCoroutine(WaitForTenSecond());

        }


        

        IEnumerator WaitForTenSecond()
        {
           

            yield return new WaitForSeconds(3f);
            Debug.Log("3��");

            time = 0f;
            
           
            if (GogglesObj.gameObject.activeSelf == true) //��ۿ�����Ʈ Ȱ��ȭ ���� üũ
            {
                //���İ��� �ٿ�����
                while (alpha_goggles.a > 0f)
                {
                    time += Time.deltaTime / frameTime;
                    alpha_goggles.a = Mathf.Lerp(1, 0f, time);
                   
                    gogglesImage.color = alpha_goggles;
                   

                }
                
            }

            if (ShieldObj.gameObject.activeSelf == true)
            {
                while (alpha_shield.a > 0f)
                {
                    time += Time.deltaTime / frameTime;
                    
                    alpha_shield.a = Mathf.Lerp(1, 0f, time);
                   
                   
                    shieldImage.color = alpha_shield;
                   
                }
                
            }
            //�ʱ�ȭ

            // time = 0f;


            
            yield return null; 
        }
    }
   
}
