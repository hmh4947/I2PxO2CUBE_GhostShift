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

    //�⺻ ����
    void Init();
    //�⺻ ����2
    void SetBasicComponent();
    //�߷�
    public void Gravity();
    //�̵�
    public void Move();
    //����
    public void Jump();
    //�뽬

    public void ChangePlayer();
}

