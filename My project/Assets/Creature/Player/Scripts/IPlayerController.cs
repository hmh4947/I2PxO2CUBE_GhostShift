using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public interface IPlayerController
{
    public float MaxSpeed { get; set; }
    public float jumpPower { get; set; }

    public Rigidbody2D rigid { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }
    public Animator anim { get; set; }
    public CapsuleCollider2D playerCollider { get; set; }

    //기본 세팅
    void Init();
    //기본 세팅2
    void SetBasicComponent();
    //중력
    public void Gravity();
    //이동
    public void Move();
    //점프
    public void Jump();
    //대쉬

    public void ChangePlayer();
}

