using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor.SearchService;
//using UnityEditorInternal;

public class SceneFadeInOut : MonoBehaviour
{

    private GameObject playerObj;
    public Image Panel;
    float time = 0f;
    float frameTime = 1f;

    public static SceneFadeInOut instance = null;

    bool coroutineControl=false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        playerObj = GameObject.Find("Player");
    }

    void Strat()
    {
        
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;

        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Fade()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlow());

    }
    public void Fade2()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlow2());

    }
    public void ReturnMap()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlowReturnMap());

    }
    public void ReturnMap2()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlowReturnMap2());

    }
    IEnumerator FadeFlow()
    {
        Debug.Log("다음맵");
        //NextMap의 자식 오브젝트 Gate 찾기
        GameObject otherGameObject = GameObject.Find("NextMap");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate 오브젝트에서 NextMap 스크립트에서 scene 변수 접근
        NextMap NextMapScript = GateTransform.GetComponent<NextMap>();
        SceneManagerEx.Scenes sceneName = NextMapScript.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //다음 씬 로드
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(sceneName);
        }
        yield return new WaitForSeconds(0.5f);

        //현재 씬이 게이트 씬과 같다면 해당되는 위치로 플레이어를 이동
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Stage1_Map2"|| currentSceneName == "Stage1_Map4")
        {

           // Debug.Log("다음 맵인 map2로 이동합니다.");
            playerObj.transform.position = new Vector2(-16, -3);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map3")
        {
            Debug.Log("다음 맵인 map3로 이동합니다.");
            playerObj.transform.position = new Vector2(-16, 5.4f);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map5")
        {
            playerObj.transform.position = new Vector2(-16, -4);
            coroutineControl = false;
        }
        if ( currentSceneName == "Stage1_Map7" || currentSceneName == "Stage1_Map8")
        {
            playerObj.transform.position = new Vector2(-16, -4);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map6")
        {

            playerObj.transform.position = new Vector2(-16, 5);
            coroutineControl = false;
        }
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);

      

        yield return null;
    }

    //Stage1_Map4에서 갈래길을 처리하기위함
    IEnumerator FadeFlow2()
    {

        //SceneChangeAreaTwo의 자식 오브젝트 Gate 찾기
        GameObject otherGameObject = GameObject.Find("NextMap_gate2");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate 오브젝트에서 SceneChangeArea 스크립트에서 scene 변수 접근
        NextMap_gate2 NextMap_gate2Script = GateTransform.GetComponent<NextMap_gate2>();
        SceneManagerEx.Scenes sceneName = NextMap_gate2Script.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //씬 로드
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(sceneName);
        }
        yield return new WaitForSeconds(0.5f);

        //현재 씬이 게이트 씬과 같다면 해당되는 위치로 플레이어를 이동
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Stage1_Map2" || currentSceneName == "Stage1_Map4")
        {


            playerObj.transform.position = new Vector2(-16, -3);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map3" || currentSceneName == "Stage1_Map6")
        {

            playerObj.transform.position = new Vector2(-16, 5);
            coroutineControl = false;
        }
        if(currentSceneName == "Stage1_Map7")
        {

            playerObj.transform.position = new Vector2(-16, 7);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map5" ||  currentSceneName == "Stage1_Map8")
        {
            playerObj.transform.position = new Vector2(-16, -4);
            coroutineControl = false;
        }
     
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);



        yield return null;
    }
    
    
    //Stage1_Map7에서 갈래길을 처리하기 위함

    IEnumerator FadeFlowReturnMap()

    {
        Debug.Log("이전맵");
        //ReturnMap의 자식 오브젝트 Gate 찾기
        GameObject otherGameObject = GameObject.Find("PreviousMap");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate 오브젝트에서 NextMap 스크립트에서 scene 변수 접근
        PreviousMap PreviousMapScript = GateTransform.GetComponent<PreviousMap>();
        SceneManagerEx.Scenes pre_sceneName = PreviousMapScript.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //씬 로드
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(pre_sceneName);
        }
        Debug.Log("씬 로드");
        yield return new WaitForSeconds(0.5f);
       
        //현재 씬이 게이트 씬과 같다면 해당되는 위치로 플레이어를 이동
        string currentSceneName = SceneManager.GetActiveScene().name;
      
            if (currentSceneName == "Stage1_Map1")
            {
                playerObj.transform.position = new Vector2(10, -3);
                Debug.Log("이전맵인 map1로 돌아갑니다.");
                coroutineControl = false;
             }
            if (currentSceneName == "Stage1_Map2")
            {
                playerObj.transform.position = new Vector2(10, 3f);
                Debug.Log("이전맵인 map2로 돌아갑니다.");
                coroutineControl = false;
            }
            if (currentSceneName == "Stage1_Map3" || currentSceneName == "Stage1_Map7")
            {

                playerObj.transform.position = new Vector2(10, -3.7f);
                Debug.Log("이전맵인 map3로 돌아갑니다.");
                coroutineControl = false;
             }
            if (currentSceneName == "Stage1_Map4")//map4 down gate
            {

                playerObj.transform.position = new Vector2(20, -3f);
                Debug.Log("이전맵인 map4로 돌아갑니다." + currentSceneName);
                coroutineControl = false;

        }
            if (currentSceneName == "Stage1_Map5")
            {
                playerObj.transform.position = new Vector2(37, -3f);
                coroutineControl = false;
            }

        

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);



        yield return null;
    }
    IEnumerator FadeFlowReturnMap2()
    {
        Debug.Log("이전맵");
        //ReturnMap의 자식 오브젝트 Gate 찾기
        GameObject otherGameObject = GameObject.Find("PreviousMap_gate2");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate 오브젝트에서 NextMap 스크립트에서 scene 변수 접근
        PreviousMap_gate2 PreviousMap_gate2Script = GateTransform.GetComponent<PreviousMap_gate2>();
        SceneManagerEx.Scenes pre_sceneName = PreviousMap_gate2Script.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //씬 로드
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(pre_sceneName);
        }
        yield return new WaitForSeconds(0.5f);

        //현재 씬이 게이트 씬과 같다면 해당되는 위치로 플레이어를 이동
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Stage1_Map4")
        {
            playerObj.transform.position = new Vector2(20, 8.5f);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map6")
        {

            playerObj.transform.position = new Vector2(22, 1.3f);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map7")
        {
            playerObj.transform.position = new Vector2(10, -3.5f);
            coroutineControl = false;
        }

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);



        yield return null;
    }
}
