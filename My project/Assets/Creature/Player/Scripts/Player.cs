using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getHP() {
        return HP;
    }
    public void Damaged(int damage)
    {
        Debug.Log("Damaged!");
        HP -= damage;
    }
}
