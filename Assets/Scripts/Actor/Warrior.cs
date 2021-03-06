﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warrior : Actor
{
    private bool isInvincible = false;//是否无敌
    private float InvincibleTime = 5f;//无敌时间
    private float InvincibleTimer = -1;//无敌时间计时器

    private int money;
    private Item mainWeapon;
    private Item asistWeapon;
    public float waveTime;
    WaveAttack waveattack;
    private int countAttack;
    public Slider blood;
    #region 生命值相关
    public void encounterAttack(Attack attack)//受到攻击
    {
        ChangeHealth(attack.damage);
    }

    public void ChangeHealth(int num)//改变生命值相关
    {
        if (num < 0)//是否为受到伤害
        {
            if (isInvincible) return;//是否无敌
            if (currentHP + num <= 0)
            {
                Debug.Log("Defeated");
            }
            InvincibleTimer = InvincibleTime;//开始无敌时间计时
            isInvincible = true;//当前状态为无敌
        }
        currentHP = Mathf.Clamp(currentHP + num, 0, totalHP);//在指定范围内改变生命值
        blood.value = (float)currentHP / totalHP;
        Debug.Log(blood.value);
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
        if (mainWeapon != null)
        {
            if (asistWeapon != null)
            {
                mainWeapon = Weapon;
            }
            else
            {
                asistWeapon = Weapon;
            }
        }
        else
        {
            mainWeapon = Weapon;
        }
    }
    #endregion

    #region 近战攻击相关
    public void setWave()
    {
        countAttack++;
        Debug.Log(countAttack);
        isAttacking = true;
        if (lastMoveDirection.x == 0 && lastMoveDirection.y == 0)
        {
            animator.SetTrigger("Wave_Down");
            waveattack = GameObject.FindGameObjectWithTag("hitBox_Down").GetComponent<WaveAttack>();
            waveattack.Attack();
        }
        else if (Math.Abs(lastMoveDirection.x) > Math.Abs(lastMoveDirection.y))
        {
            if (lastMoveDirection.x > 0)
            {
                animator.SetTrigger("Wave_Right");
                waveattack = GameObject.FindGameObjectWithTag("hitBox_Right").GetComponent<WaveAttack>();
                waveattack.Attack();
            }
            else if (lastMoveDirection.x < 0)
            {
                animator.SetTrigger("Wave_Left");
                waveattack = GameObject.FindGameObjectWithTag("hitBox_Left").GetComponent<WaveAttack>();
                waveattack.Attack();
            }
        }
        else if (Math.Abs(lastMoveDirection.x) < Math.Abs(lastMoveDirection.y))
        {
            if (lastMoveDirection.y > 0)
            {
                animator.SetTrigger("Wave_Up");
                waveattack = GameObject.FindGameObjectWithTag("hitBox_Up").GetComponent<WaveAttack>();
                waveattack.Attack();
            }
            else if (lastMoveDirection.y < 0)
            {
                animator.SetTrigger("Wave_Down");
                waveattack = GameObject.FindWithTag("hitBox_Down").GetComponent<WaveAttack>();
                waveattack.Attack();
            }
        }
        StartCoroutine(waitForWaveTime());
    }
    IEnumerator waitForWaveTime()
    {
        yield return new WaitForSeconds(waveTime);
        isAttacking = false;
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
        blood.value = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            isMoving = false;
        }
        else
            SetStand();
        CountInvincible();
    }
}
