using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private GameObject player;
    public float effectTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(GenerateEffects());

    }

    public IEnumerator GenerateEffects()
    {
        transform.position = player.transform.position;
        yield return new WaitForSeconds(effectTime);
        this.gameObject.SetActive(false);
    }
}
