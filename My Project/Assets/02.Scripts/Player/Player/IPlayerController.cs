using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public interface IPlayerController
{

    //기본 세팅
    void Init();
    //기본 세팅2
    void SetCashComponent();
    //중력
    public void Gravity();
    //이동
    public void Move();
    //리소스로드
    public void LoadResources();
}

