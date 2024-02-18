using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditorInternal;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;

public class HeartGenerator : MonoBehaviour
{
    public GameObject enemy_ghost;
    public Transform enemy;
    public GameObject HeartPrefab;//�������� �޾ƿ�
    public GameObject[] PrefabControl;//������ �������� �����ϴ� �迭

    private int randNum;
    bool count; // ������ ������ �����ϱ� ���� ����


    void Start()
    {
        randNum = Random.Range(1, 101); //1���� 100������ ���ڸ� �������� �޾ƿ´�
        Debug.Log(randNum);
        count = true;


    }

    // Update is called once per frame
    void Update()
    {


        if (enemy_ghost == null)
        {

            //20�ۼ�Ʈ Ȯ��

            if (randNum <= 20)
            {


                if (count == true)
                {
                    PrefabControl = GameObject.FindGameObjectsWithTag("Heart");

                    //enemy_ghost�� ���� ��ġ�� �����ͼ� ���ο� �������� ��ġ�� ����
                    Vector2 enemyGhostPosition = enemy.transform.position;
                    Debug.Log(enemyGhostPosition);

                    Instantiate(HeartPrefab, new Vector2(enemyGhostPosition.x, enemyGhostPosition.y), Quaternion.identity); //������ ����

                    count = false;
                }


            }
            else
            {
                // Debug.Log("��Ʈ�� �������� ����");
            }

        }
    }


}
