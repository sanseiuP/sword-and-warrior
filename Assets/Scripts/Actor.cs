using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    private string name;
    private int totalHP, currentHP;
    private float speed = 3f;
    public float test = 0;
    Transform transform;//获取人物位置信息
    Rigidbody2D rigidbody;//人物刚体模型
    Animator animator;//控制动画相关
    Attack attack;

    public void setMove(float direction)//设置移动，方向由弧度制表示
    {
        animator.SetBool("isMoving", true);
        Vector2 moveDirection = new Vector2(1, 0);//移动方向，默认朝右
        moveDirection.x = (float)Math.Cos(direction);
        moveDirection.y = (float)Math.Sin(direction);
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", moveDirection.magnitude);
        transform.Translate(moveDirection * speed * Time.deltaTime);//使人物朝移动方向每帧移动speed的长度
    }

    public void setStand(int action)//设置人物静置
    {
        animator.SetBool("isMoving", false);
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
