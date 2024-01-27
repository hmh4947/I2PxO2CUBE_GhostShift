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
       

      /*  //image 컴포넌트 가져오기
        Image gogglesImage=GetComponent<Image>();
        Image shieldImage = GetComponent<Image>();
        Image cleanerImage = GetComponent<Image>();

        */

    }

    // Update is called once per frame
    void Update()
    {
        Transform parentTransform = transform;
        Transform GhostObj = parentTransform.GetChild(0); // Player의 자식 오브젝트 중 Ghost
        Transform ShieldObj = parentTransform.GetChild(1); // Player의 자식 오브젝트 중 Shield
        Transform GogglesObj = parentTransform.GetChild(2); // Player의 자식 오브젝트 중 Goggle
        Transform CleanerObj = parentTransform.GetChild(5); // Player의 자식 오브젝트 중 Cleaner
                                                          

        Color alpha_goggles = gogglesImage.color;
        Color alpha_shield = shieldImage.color;
        Color alpha_cleaner = cleanerImage.color;

        
        if (GhostObj != null && GhostObj.gameObject.activeSelf == true)//GhostObj 활성화 여부 
        {
          
            Debug.Log("고스트");
            goggles_guide.SetActive(false);
            shield_guide.SetActive(false);
            cleaner_guide.SetActive(false);
            StopCoroutine(WaitForTenSecond());
            shieldImage.color = new Color(255, 255, 255, 1); //알파값 초기화
                                                           

            // 자식 객체가 활성화되어 있음
            //  Debug.Log("고스트");

        }
        if (ShieldObj != null && ShieldObj.gameObject.activeSelf == true)
        {

            // 가이드 ui
            shield_guide.SetActive(true);


            //10초 기다리기
            StartCoroutine(WaitForTenSecond());
            //StopCoroutine(WaitForTenSecond());
            
            Debug.Log("쉴드");
        }

        if (GogglesObj != null && GogglesObj.gameObject.activeSelf == true)
        {
            // 자식 객체가 활성화되어 있음

            goggles_guide.SetActive(true);
            Debug.Log("고글");
            StartCoroutine(WaitForTenSecond());

        }


        if (CleanerObj != null && CleanerObj.gameObject.activeSelf == true)
        {
            // 자식 객체가 활성화되어 있음
            cleaner_guide.SetActive(true);

            Debug.Log("청소기");

            StartCoroutine(WaitForTenSecond());

        }


        

        IEnumerator WaitForTenSecond()
        {
           

            yield return new WaitForSeconds(3f);
            Debug.Log("3초");

            time = 0f;
            
           
            if (GogglesObj.gameObject.activeSelf == true) //고글오브젝트 활성화 여부 체크
            {
                //알파값을 줄여나감
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
            //초기화

            // time = 0f;


            
            yield return null; 
        }
    }
   
}
