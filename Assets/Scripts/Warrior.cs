using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Actor
{
    private bool isInvincible = false;//是否无敌
    private float InvincibleTime = 5f;//无敌时间
    private float InvincibleTimer = -1;//无敌时间计时器

    private int money;
    private Item mainWeapon;
    private Item asistWeapon;

    Transform transform;
    Rigidbody2D rigidbody;
    Animator animator;
    AudioSource audioSource;

    #region 生命值相关
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
    #endregion

    #region 金钱相关
    public void changeMoney(int num)//改变金钱
    {
        money = Mathf.Clamp(money + num, 0, 9999);
    }
    #endregion

    #region 武器相关
    public void setWeapon(Item Weapon)
    {

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
