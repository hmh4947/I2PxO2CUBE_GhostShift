using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public Animator ani;

    public TypewriterByCharacter textAnimatorPlayer;
    [TextArea(5, 50), SerializeField] //인스펙터에 접근 가능(외부 스크립트에서는 불가능)
    string textToShow = " ";
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        
    }
    public void ShowText()
    {
        textAnimatorPlayer.ShowText(textToShow);
    }
    public void message()
    {
        
        ani.SetBool("message", true);
       // Debug.Log("메시지 보이기");
        ShowText(); //메세지 박스가 보일때 마다 텍스트 애니메이션이 동작
    }
    public void messageBehind()
    {
     
        ani.SetBool("message", false);
        
       // Debug.Log("메시지 가리기");
        
    }
    // Update is called once per frame
    public void OnBecameInvisible() //오브젝트가 카메라 밖에 있는지 체크
    {
      //  Debug.Log("안보임");
      
    }
    public void OnBecameVisible() //오브젝트가 카메라 안에 있는지 체크
    {
     //   Debug.Log("보임");
        
    }
}
