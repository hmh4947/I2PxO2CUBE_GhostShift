using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;

public class HeartGenerator : MonoBehaviour
{
    public GameObject enemy_ghost;
    public Transform enemy;
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
        
           

        if (enemy_ghost == null)
        {

            //20퍼센트 확률
           
            if (randNum <= 20)
            {
               
                if (count == true)
                {
                    PrefabControl = GameObject.FindGameObjectsWithTag("Heart");

                   //PrefabControl[0] = Instantiate(HeartPrefab); //프리팹을 배열에 넣음

                    if (PrefabControl.Length < 1)
                    {
                        // enemy_ghost의 현재 위치를 가져와서 새로운 프리팹의 위치로 설정
                        Vector2 enemyGhostPosition = enemy.transform.position;


                        PrefabControl[0] = Instantiate(HeartPrefab, new Vector2(enemyGhostPosition.x, enemyGhostPosition.y + 1.5f),Quaternion.identity); //프리팹을 배열에 넣음



                        Debug.Log("하트 생성");
                    }
                    count = false;
                }


            }
            else
            {
                // Debug.Log("하트를 생성하지 않음");
            }

        }
    }
}
