using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public interface IPlayerController
{

    //�⺻ ����
    void Init();
    //�⺻ ����2
    void SetCashComponent();
    //�߷�
    public void Gravity();
    //�̵�
    public void Move();
    //�뽬
}

