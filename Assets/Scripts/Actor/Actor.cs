using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //人物数据相关
    protected string name;
    protected int totalHP, currentHP;
    public float speed = 3f;
    protected bool isMoving = false;
    protected Vector2 lastMoveDirection;//上一次移动方向
    protected Vector2 moveDirection = new Vector2(0, 0);//移动方向

    //音频相关
    AudioSource audioSource;
    public AudioClip Footsteps;

    
    

    Transform transform;//获取人物位置信息
    Rigidbody2D rigidbody;//人物刚体模型
    Animator animator;//控制动画相关
    Attack attack;
    
    #region 人物移动相关
    public void SetMove(float direction, Rigidbody2D rigidbody, Animator animator,AudioSource audioSource)//设置移动，方向由弧度制表示
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

    public void SetStand(Rigidbody2D rigidbody, Animator animator, AudioSource audioSource)//设置人物静置
    {
        moveDirection = Vector2.zero;
        rigidbody.velocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        audioSource.Pause();
    }
    #endregion

    #region 改变生命值相关
    public void changeHealth(int amount)
    {
        if (currentHP + amount < totalHP)
        {
            Destroy(this.gameObject);
        }
        else
        {
            currentHP = Mathf.Clamp(currentHP + amount, 0, totalHP);
        }
    }
    public void EncounterAttack(Attack attack)//受到攻击
    {
        changeHealth(attack.damage);
    }
    #endregion

 
    // Start is called before the first frame update
    void Start()
    {
        //获取组件
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHP = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            isMoving = false;
        }
        else
            SetStand(rigidbody, animator, audioSource);
    }
    
}
