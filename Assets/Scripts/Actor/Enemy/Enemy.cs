using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    protected bool moving;
    protected float moveCounter;
    protected float moveTime = 5f;
    //扇形检测
    protected float lookAccurte = 3f;//扇形精度
    protected float lookAngle = 90f;//扇形角度
    protected float rotatePerSecond = 90f;//每一帧旋转角度

    //自动寻路相关
    public GameObject target;
    public float nextWaypointDistance = 0.3f;
    protected Path path;
    protected int currentWaypoint;
    protected bool reachEndOfPath = false;
    Seeker enemyseeker;
    protected bool isFound = false;

    Rigidbody2D enemyrigidbody;//人物刚体模型
    Animator enemyanimator;//控制动画相关
    public void SetMove(Animator animator, Rigidbody2D rigidbody)
    {
        float chance = Random.value;
        if (!moving)
        {
            lastMoveDirection = moveDirection;
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

    public void SetStand(Animator animator, Rigidbody2D rigidbody)
    {
        rigidbody.velocity = Vector2.zero;
        moving = false;
        animator.SetBool("isMoving", false);
    }

    public void SetAutoPathFinding(Animator animator, Rigidbody2D rigidbody)
    {

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
        }
        else
        {
            reachEndOfPath = false;
        }

        float distance = 0;
        lastMoveDirection = moveDirection;

        if (currentWaypoint >= 0 && currentWaypoint < path.vectorPath.Count)
        {
            moveDirection = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
            distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);
        }
        if (moveDirection.x == 0 && moveDirection.y == 0)
        {
            animator.SetFloat("Look X", lastMoveDirection.x);
            animator.SetFloat("Look Y", lastMoveDirection.y);
        }
        else
        {
            animator.SetFloat("Look X", moveDirection.x);
            animator.SetFloat("Look Y", moveDirection.y);
        }
        animator.SetBool("isMoving", true);

        rigidbody.velocity = moveDirection * speed;

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public void Detect(Animator animator, Rigidbody2D rigidbody)
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

    public void MoveControl(Animator animator, Rigidbody2D rigidbody)
    {
        moveCounter -= 1;
        if (moving)
        {
            if (moveCounter < 0)
            {
                SetStand(animator, rigidbody);
            }
        }
        else
        {
            if (moveCounter < -3f)
            {
                moveCounter = moveTime;
                SetMove(animator, rigidbody);
            }
        }
    }

    public IEnumerator UpdatePath(Seeker seeker, Rigidbody2D rigidbody)
    {
        while (true)
        {
            //if (isFound)
            {
                if (seeker.IsDone())
                {
                    seeker.StartPath(rigidbody.position, target.GetComponent<Rigidbody2D>().position, OnPathComplete);
                }
            }
            yield return new WaitForSeconds(0.5f);
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
        enemyrigidbody = GetComponent<Rigidbody2D>();
        enemyanimator = GetComponent<Animator>();
        enemyseeker = GetComponent<Seeker>();

        StartCoroutine(UpdatePath(enemyseeker, enemyrigidbody));

        moving = false;
        isFound = false;
        moveCounter = moveTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFound)
        {
            MoveControl(enemyanimator, enemyrigidbody);
            Detect(enemyanimator, enemyrigidbody);
        }
        else
        {
            SetAutoPathFinding(enemyanimator, enemyrigidbody);
        }
    }
}
