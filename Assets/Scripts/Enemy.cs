using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private bool moving;
    private bool isRising;
    private float moveCounter;
    private float moveTime = 5f;
    //扇形检测
    private float lookAccurte = 3f;//扇形精度
    private float lookAngle = 90f;//扇形角度
    private float rotatePerSecond = 90f;//每一帧旋转角度

    new Rigidbody2D rigidbody;//人物刚体模型
    Animator animator;//控制动画相关
    public void SetMove()
    {
        float chance = Random.value;
        if (!moving)
        {
            if (chance < 0.25)
            {
                moveDirection = Vector2.left;
            }
            else if (chance >= 0.25 && chance < 0.5)
            {
                moveDirection = Vector2.right;
            }
            else if (chance >= 0.5 && chance < 0.75)
            {
                moveDirection = Vector2.up;
            }
            else if (chance >= 0.75 && chance < 1)
            {
                moveDirection = Vector2.down;
            }
            animator.SetFloat("Look X", moveDirection.x);
            animator.SetFloat("Look Y", moveDirection.y);
            animator.SetBool("isMoving", true);
            rigidbody.velocity = moveDirection * speed;
            moving = true;
        }
    }

    public new void SetStand()
    {
        rigidbody.velocity = Vector2.zero;
        moving = false;
        animator.SetBool("isMoving", false);
    }

    public void Detect()
    {
        float subAngle = (lookAngle / 2) / lookAccurte;
        Physics2D.queriesStartInColliders = false;
        for (int i = 0; i < lookAccurte; i++)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(rigidbody.position, Quaternion.Euler(0, 0, -lookAngle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle)) * moveDirection, 8f);
            Debug.DrawRay(rigidbody.position + Vector2.up, Quaternion.Euler(0, 0, -lookAngle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle)) * moveDirection, Color.red);
            if (hit2D.collider != null)
            {
                Actor player = hit2D.collider.GetComponent<Actor>();
                if (player != null)
                {
                    GameObject.Find("Enemy").GetComponent<AIPath>().enabled = true;
                    Debug.Log("Emeny spoted");
                }
                else
                {
                    SetStand();
                }
            }
        }
    }

    public void MoveControl()
    {
        moveCounter -= 1;
        if (moving)
        {
            if (moveCounter < 0)
            {
                SetStand();
            }
        }
        else
        {
            if (moveCounter < -3f)
            {
                moveCounter = moveTime;
                SetMove();
            }
        }
    }

    public void Apathfinding()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moving = false;
        moveCounter = moveTime;
        GameObject.Find("Enemy").GetComponent<AIPath>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveControl();
        Detect();
    }
}
