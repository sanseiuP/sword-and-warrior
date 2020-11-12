using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private bool moving;
    private float moveCounter;
    private float moveTime = 5f;
    //扇形检测
    private float lookAccurte = 3f;//扇形精度
    private float lookAngle = 90f;//扇形角度
    private float rotatePerSecond = 90f;//每一帧旋转角度

    //自动寻路相关
    public Actor target;
    public float nextWaypointDistance = 0.3f;
    Path path;
    int currentWaypoint;
    bool reachEndOfPath = false;
    Seeker seeker;
    private bool isFound = false;

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

    public void SetAutoPathFinding()
    {

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
        }
        else
        {
            reachEndOfPath = false;
        }

        moveDirection = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetBool("isMoving", true);

        rigidbody.velocity = moveDirection * speed;

        float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
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
                    isFound = true;
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

    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rigidbody.position, target.GetComponent<Rigidbody2D>().position, OnPathComplete);
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        moving = false;
        isFound = false;
        moveCounter = moveTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFound)
        {
            MoveControl();
            Detect();
        }
        else
        {
            SetAutoPathFinding();
        }
    }
}
