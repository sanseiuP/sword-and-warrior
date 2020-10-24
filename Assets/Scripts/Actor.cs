using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //人物数据相关
    private string name;
    private int totalHP, currentHP;
    private float speed = 3f;
    bool isMoving = true;

    //音频相关
    private AudioSource audioSource;
    public AudioClip Footsteps;

    Transform transform;//获取人物位置信息
    Rigidbody2D rigidbody;//人物刚体模型
    Animator animator;//控制动画相关
    Attack attack;

    public void setMove(float direction)//设置移动，方向由弧度制表示
    {
        Vector2 moveDirection = new Vector2(1, 0);//移动方向，默认朝右
        moveDirection.x = (float)Math.Cos(direction);
        moveDirection.y = (float)Math.Sin(direction);
        //移动动画相关
        animator.SetBool("isMoving", true);
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        //移动音频相关
        if (!audioSource.isPlaying)
            audioSource.Play();
        transform.Translate(moveDirection * speed * Time.deltaTime);//使人物朝移动方向每帧移动speed的长度
    }

    public void setStand()//设置人物静置
    {
        animator.SetBool("isMoving", false);
        audioSource.Pause();
    }

    private void encounterAttack(Attack attack)//发出攻击
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //获取组件
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {/*
        if (isMoving)
        {
            setMove(0);
            if (Input.GetKeyDown(KeyCode.F))
            {
                setStand();
                isMoving = false;
            }
        }*/
    }
}
