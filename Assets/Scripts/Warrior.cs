using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    //人物数据相关
    private string name;
    private int totalHP, currentHP;
    public float speed = 10f;
    private bool isMoving = false;
    private bool isInvincible = false;//是否无敌
    private float InvincibleTime = 5f;//无敌时间
    private float InvincibleTimer = -1;//无敌时间计时器
    Vector2 lastMoveDirection;//上一次移动方向
    Vector2 moveDirection = new Vector2(0, 0);//移动方向

    //音频相关
    private AudioSource audioSource;
    public AudioClip Footsteps;


    Transform transform;//获取人物位置信息
    Rigidbody2D rigidbody;//人物刚体模型
    Animator animator;//控制动画相关
    Attack attack;
    public void setMove(float direction)//设置移动，方向由弧度制表示
    {
        isMoving = true;
        moveDirection.x = (float)Math.Cos(direction);
        moveDirection.y = (float)Math.Sin(direction);
        //移动动画相关
        animator.SetBool("isMoving", true);
        if (lastMoveDirection != moveDirection)//通过比较上一次与今次的移动方向来使移动动画为后按的方向
        {
            if (lastMoveDirection.x == 0 && lastMoveDirection.y == 0)
            {
                animator.SetFloat("Look X", moveDirection.x);
                animator.SetFloat("Look Y", moveDirection.y);
            }
            else if (Math.Abs(lastMoveDirection.x) > Math.Abs(lastMoveDirection.y))
            {
                if (moveDirection.y >= 0)
                {
                    animator.SetFloat("Look X", moveDirection.x);
                    animator.SetFloat("Look Y", moveDirection.y + 0.1f);
                }
                else
                {
                    animator.SetFloat("Look X", moveDirection.x);
                    animator.SetFloat("Look Y", moveDirection.y - 0.1f);
                }
            }
            else if (Math.Abs(lastMoveDirection.x) <= Math.Abs(lastMoveDirection.y))
            {
                if (moveDirection.x >= 0)
                {
                    animator.SetFloat("Look X", moveDirection.x + 0.1f);
                    animator.SetFloat("Look Y", moveDirection.y);
                }
                else
                {
                    animator.SetFloat("Look X", moveDirection.x - 0.1f);
                    animator.SetFloat("Look Y", moveDirection.y);
                }
            }
            lastMoveDirection = moveDirection;
        }
        //移动音频相关
        if (!audioSource.isPlaying)
            audioSource.Play();
        Vector2 position = rigidbody.position;
        position += moveDirection * speed * Time.deltaTime;
        rigidbody.velocity = speed * moveDirection;//使人物朝移动方向每帧移动speed的长度
    }

    public void setStand()//设置人物静置
    {
        moveDirection = Vector2.zero;
        rigidbody.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        audioSource.Pause();
    }

    public void encounterAttack(Attack attack)//受到攻击
    {
        changeHealth(attack.damage);
    }

    public void changeHealth(int num)//改变生命值相关
    {
        if (num < 0)//是否为受到伤害
        {
            if (isInvincible) return;//是否无敌
            InvincibleTimer = InvincibleTime;//开始无敌时间计时
            isInvincible = true;//当前状态为无敌
        }
        currentHP = Mathf.Clamp(currentHP + num, 0, totalHP);//在指定范围内改变生命值
    }

    public void CountInvincible()//无敌时间计时
    {
        InvincibleTimer -= Time.deltaTime;
        if (InvincibleTimer < 0)
            isInvincible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //获取组件
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        totalHP = 10;
        currentHP = totalHP;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            isMoving = false;
        }
        else
            setStand();
        CountInvincible();
    }
}
