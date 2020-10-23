using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    private string name;
    private int HP;
    private float speed = 5f;
    Transform transform;//获取人物位置信息
    Rigidbody2D rigidbody;//人物刚体模型
    Attack attack;

    public void setMove(float direction)//设置移动，方向由弧度制表示
    {
        Vector2 moveDirection = new Vector2(1, 0);//移动方向，默认朝右
        moveDirection.x = (float)Math.Cos(direction);
        moveDirection.y = (float)Math.Sin(direction);
        transform.Translate(moveDirection * speed * Time.deltaTime);//使人物朝移动方向每帧移动speed的长度
    }

    public void setAction(int action)//设置动作
    {

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
