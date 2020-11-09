﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private bool moving;
    private float moveCounter;
    private float moveTime = 1f;

    new Rigidbody2D rigidbody;//人物刚体模型
    Animator animator;//控制动画相关
    public void setMove()
    {
        float chance = Random.value;
        if (!moving)
        {
            if (chance < 0.25)
                moveDirection = Vector2.left;
            else if (chance >= 0.25 && chance < 0.5)
                moveDirection = Vector2.right;
            else if (chance >= 0.5 && chance < 0.75)
                moveDirection = Vector2.up;
            else if (chance >= 0.75 && chance < 1)
                moveDirection = Vector2.down;
            rigidbody.velocity = moveDirection * speed;
            moving = true;
            moveCounter = moveTime;
        }
    }

    public void detect()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(rigidbody.position, moveDirection, 2f);
        if (hit2D.collider != null)
        {
            Actor player = hit2D.collider.GetComponent<Actor>();
            if (player != null)
            {
                Debug.Log("Emeny spoted");
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
                moving = false;
            }
        }
    }

    public void countMoving()
    {
        if (moving)
        {
            moveCounter -= Time.deltaTime;
            if (moveCounter < 0)
            {
                rigidbody.velocity = Vector2.zero;
                moving = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moving = false;
        moveCounter = moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        setMove();
        detect();
        countMoving();
    }
}
