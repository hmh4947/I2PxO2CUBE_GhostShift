using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();

    }

    public void message()
    {
        ani.SetBool("message", true);
        Debug.Log("�޽��� ���̱�");
    }
    public void messageBehind()
    {
        ani.SetBool("message", false);
        
        Debug.Log("�޽��� ������");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
