using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private int HP;
    [SerializeField] 
    private int numOfHearts;

    // 무적 상태에 돌입했는지 확인하는 bool 변수
    public bool isInvincible;

    // 무적시간
    public float invincibleTime;
    // 깜빡이는 시간
    public float blinkingTime;

    public Image[] hearts;
    public Sprite fulltHeart;
    public Sprite emptyHeart;

    private SpriteRenderer playerSpr;
    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
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
    /// <summary>
    /// 데미지 입는 함수, 무적시간에 들어가있을 경우 반환값은 false 아닐 경우 true
    /// </summary>
    /// <param name="damage">데미지 양</param>
    /// <returns></returns>
    public bool Damaged(int damage)
    {
        if (!isInvincible)
        {
            HP -= damage;
            if(HP <= 0)
            {
                SceneManagerEx.Instance.LoadScene(SceneManagerEx.Scenes.Title);
            }
            StartCoroutine(Invincible());
            return true;
        }
        return false;

    }
    public void Healed(int health)
    {
        HP += health;
    }

    public IEnumerator Invincible()
    {
        isInvincible = true;
        StartCoroutine(Blinking());
        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    public IEnumerator Blinking()
    {
        playerSpr = GetComponentInChildren<SpriteRenderer>();
        while (isInvincible)
        {
            playerSpr.color = new Color32(255, 255, 255, 20);

            yield return new WaitForSeconds(blinkingTime);

            playerSpr.color = new Color32(255, 255, 255, 255);

            yield return new WaitForSeconds(blinkingTime);
        }
    }
}
