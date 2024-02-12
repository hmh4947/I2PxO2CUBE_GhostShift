using Febucci.UI;
using Febucci.UI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject text;
    public Animator ani;
    public GameObject msgObj;
    private void Start()
    {
     
       text.SetActive(false);
       
       ani = GetComponent<Animator>();
      //  msgObj.GetComponent<Message>().OnBecameVisible();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {

            msgObj.GetComponent<Message>().message();
        //    msgObj.GetComponent<Message>().OnBecameVisible();
            text.SetActive(true);
          
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            msgObj.GetComponent<Message>().messageBehind();
           // msgObj.GetComponent<Message>().OnBecameInvisible();
          
        }

    }
}
