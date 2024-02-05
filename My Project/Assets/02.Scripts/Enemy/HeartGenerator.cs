using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;

public class HeartGenerator : MonoBehaviour
{
    public GameObject enemy_ghost;

    public GameObject HeartPrefab;//프리팹을 받아옴
    public GameObject[] PrefabControl;//생성된 프리팹을 저장하는 배열

    private int randNum;
    bool count; // 프리팹 개수를 제한하기 위한 변수

   
    void Start()
    { 
        randNum = Random.Range(1, 101); //1부터 100사이의 숫자를 랜덤으로 받아온다
        Debug.Log(randNum);
        count = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy_ghost== null)
        {
            //20퍼센트 확률

            if (randNum <= 20)
            {
                if (count == true)
                {
                    
                    PrefabControl = GameObject.FindGameObjectsWithTag("Heart");

                    if (PrefabControl.Length < 1)
                    {

                        PrefabControl[0] = Instantiate(HeartPrefab); //프리팹을 배열에 넣음
                        Debug.Log("하트 생성");
                        PrefabControl[0].transform.position = enemy_ghost.transform.position; //에너미 위치에서 생성

                        
                    }
                    count = false;
                }

                
            }
            else
            {
                Debug.Log("하트를 생성하지 않음");
            }
            
        }
    }
}
