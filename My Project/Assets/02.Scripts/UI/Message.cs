using Febucci.UI;
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
        Debug.Log("메시지 보이기");
       
    }
    public void messageBehind()
    {
     
        ani.SetBool("message", false);
        
        Debug.Log("메시지 가리기");
        
    }
    // Update is called once per frame
    public void OnBecameInvisible() //오브젝트가 카메라 밖에 있는지 체크
    {
        Debug.Log("안보임");
      
    }
    public void OnBecameVisible() //오브젝트가 카메라 안에 있는지 체크
    {
        Debug.Log("보임");
        
    }
}
