using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnPlayerHit : MonoBehaviour
{
    private float m_roughtness=1f;
    private float m_magnitude=1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  
    // Update is called once per frame
    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Shake(1f));
            Debug.Log("shake");
        }    
    }
    IEnumerator Shake(float duration)
    {
        float halfDurantion = duration / 2;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);
        while(elapsed<duration)
        {
            elapsed += Time.deltaTime / halfDurantion;

            tick += Time.deltaTime * m_roughtness;
            transform.position = new Vector3(
                Mathf.PerlinNoise(tick, 0) - .5f,
                Mathf.PerlinNoise(0, tick) - .5f, 0f) * m_magnitude * Mathf.PingPong(elapsed, halfDurantion);

            yield return null;
        }
    }
    
}
