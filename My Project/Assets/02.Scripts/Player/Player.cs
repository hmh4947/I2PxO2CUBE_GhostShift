using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{





    // �÷��̾� HP
    private int HP;

    // �÷��̾� ���� ����
    private bool isPossesing;

    public bool IsPossesing
    {
        get
        {
            return isPossesing;
        }
        set
        {
            this.isPossesing = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isPossesing = false;
        HP = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getHP() {
        return HP;
    }
    
    // ���� �ð�
}
