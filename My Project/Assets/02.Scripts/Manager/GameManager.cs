using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * 싱글톤을 활용한 MonoBehaviour를 상속받는 게임매니저 구현
 * 유니티 싱글톤 개념 유튜브 링크: https://www.youtube.com/watch?v=-wzULWMvFu0
 * */
public class GameManager : MonoBehaviour
{
    // instance 변수는 단 하나만 존재해야 하기 때문에 static으로 선언
    private static GameManager instance;
    // instance property
    public static GameManager Instance
    {
        get
        {
            // instance가 비어있는지 검사
            if(instance == null)
            {
                // 씬 안에 GameManager 컴포넌트를 가지고 있는 오브젝트가 있는지 검사.
                var obj = FindObjectOfType<GameManager>();
                // 존재한다면 그 객체를 instance에 삽입
                if(obj != null)
                {
                    instance = obj;
                }
                    // 존재하지 않는다면 새로운 게임오브젝트를 생성후 그 게임오브젝트에 GameManger를 부착 후 instance에 삽입
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();
                    instance = newObj;
                }
            }
            // instance 반환
            return instance;
        }
    }

    private void Awake()
    {
        // 씬에 게임 매니저 컴포넌트를 가지고 있는 컴포넌트가 총 몇개 있는지 검사
        var objs = FindObjectsOfType<GameManager>();
        // 게임매니저 컴포넌트의 개수가 1개가 아니라면
        if(objs.Length != 1)
        {
            // 이미 instance에 게임매니저가 할당된 것이기에 방금 생성된 객체 파괴
            Destroy(gameObject);
            return;
        }
        // 씬이 넘어가도 파괴되지 않도록 DontDestroyOnLoad 적용
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {

    }

    public void EndGame()
    {

    }
}
