using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int HP;
    private int numOfHearts;

    public Image[] hearts;
    public Sprite fulltHeart;
    public Sprite emptyHeart;
    // Start is called before the first frame update
    void Start()
    {
        HP = 4;
        numOfHearts = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP > numOfHearts)
        {
            HP = numOfHearts;
        }
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < HP)
            {
                hearts[i].sprite = fulltHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public int getHP()
    {
        return HP;
    }
    public void Damaged(int damage)
    {
        Debug.Log("Damaged!");
        HP -= damage;
    }
    public void Healed(int health)
    {
        HP += health;
    }
}
