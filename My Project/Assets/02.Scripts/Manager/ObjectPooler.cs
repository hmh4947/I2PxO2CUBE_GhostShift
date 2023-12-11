using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region Instance Properties
    // EffectManager는 단 하나만 존재해야 하므로 static으로 선언
    private static ObjectPooler instance;
    // instance property
    public static ObjectPooler Instance
    {
        get
        {
            // instance가 비어있는지 검사
            if(instance == null)
            {
                // 씬 안에 EffectManager 컴포넌트를 가지고 있는 오브젝트가 있는지 검사.
                var obj = FindObjectOfType<ObjectPooler>();
                // 존재한다면 그 객체를 instance에 삽입
                if(obj != null)
                {
                    instance = obj;
                }
                // 존재하지 않는다면 새로운 게임오브젝트를 생성후 그 게임오브젝트에 EffectManager를 부착 후 instance에 삽입
                else
                {
                    var newObj = new GameObject().AddComponent<ObjectPooler>();
                    instance = newObj;
                }
            }
            // instance 반환
            return instance;
        }
    }
    #endregion

    #region Effect ObjectPool
    // 이펙트 게임오브젝트 숫자
    [SerializeField] private uint effectPoolSize;
    public uint EffectPoolSize => effectPoolSize;

    [SerializeField] public GameObject hitEffect;

    private Stack<GameObject> hitEffectPool;

    private void SetUpEffectPool()
    {
        if(hitEffect == null)
        {
            return;
        }
        hitEffectPool = new Stack<GameObject>();

        GameObject instance = null;

        for(int i = 0; i < effectPoolSize; i++)
        {
            instance = Instantiate(hitEffect);
            instance.SetActive(false);
            hitEffectPool.Push(instance);
        }
    }

    public GameObject GetEffectObject()
    {
        if(hitEffectPool == null)
        {
            return null;       
        }
        if(hitEffectPool.Count == 0)
        {
            GameObject newInstance = Instantiate(hitEffect);
            return newInstance;
        }

        GameObject nextInstance = hitEffectPool.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }
    #endregion
    // SingleTon Awake
    private void Awake()
    {
        // 씬에 EffectManager 컴포넌트를 가지고 있는 오브젝트가 총 몇개 있는지 탐색
        var objs = FindObjectsOfType<ObjectPooler>();
        // 만약 오브젝트의 수가 1이 아니라면
        if(objs.Length != 1)
        {
            // 이미 instance에 EffectManager가 할당된 것이기에 방금 생성된 객체 파괴
            Destroy(gameObject);
            return;
        }
        // 씬이 넘어가도 파괴되지 않도록 DontDestroyOnLoad 적용
        DontDestroyOnLoad(gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
