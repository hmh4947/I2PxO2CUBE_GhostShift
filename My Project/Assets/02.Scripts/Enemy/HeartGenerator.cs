using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;

public class HeartGenerator : MonoBehaviour
{
    public GameObject enemy_ghost;

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
        if (enemy_ghost== null)
        {
            //20�ۼ�Ʈ Ȯ��

            if (randNum <= 20)
            {
                if (count == true)
                {
                    
                    PrefabControl = GameObject.FindGameObjectsWithTag("Heart");

                    if (PrefabControl.Length < 1)
                    {

                        PrefabControl[0] = Instantiate(HeartPrefab); //�������� �迭�� ����
                        Debug.Log("��Ʈ ����");
                        PrefabControl[0].transform.position = enemy_ghost.transform.position; //���ʹ� ��ġ���� ����

                        
                    }
                    count = false;
                }

                
            }
            else
            {
                Debug.Log("��Ʈ�� �������� ����");
            }
            
        }
    }
}
