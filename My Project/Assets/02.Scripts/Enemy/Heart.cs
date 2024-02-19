using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    protected Health healthScr;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        healthScr =player.GetComponent<Health>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
       if( collider.tag=="Player"){
                
         
           healthScr.Healed(1);
           Debug.Log("ªË¡¶");
           Destroy(gameObject);
            
       }
       
    }
}
