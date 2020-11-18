using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class WaterSlime : Enemy
{
    float attackDistance;
    float attackRange = 3f;
    float offset = (float)System.Math.Sqrt(2.0) / 2;
    bool isFlying = false;
    float g = 10f;
    float flyTime = 1;
    float flyTimeCounter = 0;
    float flyDistance;
    float angle = 0;
    float xSpeed;
    float ySpeed;
    float x, y, z;
    Vector2 position = Vector2.zero;
    Vector2 startPosition = Vector2.zero;

    Rigidbody2D waterSlimerigidbody;
    Animator waterSlimeanimator;
    Seeker waterSlimeseeker;

    public void Detect()
    {
        float subAngle = (lookAngle / 2) / lookAccurte;
        Physics2D.queriesStartInColliders = false;
        for (int i = 0; i < lookAccurte; i++)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(waterSlimerigidbody.position, Quaternion.Euler(0, 0, -lookAngle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle)) * moveDirection, 8f);
            Debug.DrawRay(waterSlimerigidbody.position + Vector2.up, Quaternion.Euler(0, 0, -lookAngle / 2 + i * subAngle + Mathf.Repeat(rotatePerSecond * Time.time, subAngle)) * moveDirection, Color.red);
            if (hit2D.collider != null)
            {
                Actor player = hit2D.collider.GetComponent<Actor>();
                if (player != null)
                {
                    isFound = true;
                    attackDistance = Vector2.Distance(waterSlimerigidbody.position, player.GetComponent<Rigidbody2D>().position);
                }
            }
        }
    }

    public void parabolicMotion(Animator animator, Rigidbody2D rigidbody)
    {
        waterSlimeanimator.SetFloat("AttackDistance", flyDistance);
        if (!isFlying)
        {
            flyTimeCounter = 0;
            isFlying = true;
            animator.SetBool("isFlying", true);
            flyTime = flyDistance / speed;
            moveDirection = target.GetComponent<Rigidbody2D>().position - rigidbody.position;
            animator.SetFloat("Look X", moveDirection.x);
            animator.SetFloat("Look Y", moveDirection.y);
            angle = (float)(Vector2.Angle(Vector2.right, moveDirection) * System.Math.PI / 180);
            position = rigidbody.position;
            startPosition = position;
        }
        else
        {
            if (flyTimeCounter <= flyTime)
            {
                xSpeed =(float) System.Math.Cos(angle) * speed;
                ySpeed = (float)System.Math.Sin(angle) * speed;
                x = xSpeed * flyTimeCounter;
                y = ySpeed * flyTimeCounter;
                z = (float)0.5 * g * flyTimeCounter * (flyTime - flyTimeCounter);
                if (z != 0)
                {
                    if (y < z)
                    {
                        y = (float)(System.Math.Sqrt(y * y + z * z) / (2 * offset) * System.Math.Cos(System.Math.PI / 4 - System.Math.Atan(y / z)));
                    }
                    else
                    {
                        y = (y + z) / 2;
                    }
                }
                if (moveDirection.y < 0)
                {
                    y = -y;
                }
                Debug.Log("x = " + x + ", y = " + y);
                position += new Vector2(x, y);
                rigidbody.MovePosition(position);
                Debug.Log(rigidbody.position);
                flyTimeCounter += Time.deltaTime;
                position = startPosition;
            }
            else
            {
                animator.SetBool("isFlying", false);
                isFlying = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Warrior warrior = other.gameObject.GetComponent<Warrior>();
        if (warrior != null)
        {
            warrior.changeHealth(-1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        waterSlimerigidbody = GetComponent<Rigidbody2D>();
        waterSlimeanimator = GetComponent<Animator>();
        waterSlimeseeker = GetComponent<Seeker>();

        StartCoroutine(UpdatePath(waterSlimeseeker, waterSlimerigidbody));

        moving = false;
        isFound = false;
        moveCounter = moveTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isFound)
        {
            MoveControl(waterSlimeanimator, waterSlimerigidbody);
            Detect();
        }
        else
        {
            attackDistance = Vector2.Distance(waterSlimerigidbody.position, target.GetComponent<Rigidbody2D>().position);
            if (attackRange < attackDistance && !isFlying && !waterSlimeanimator.GetBool("shouldStand"))//目标不在攻击范围内
            {
                waterSlimeanimator.ResetTrigger("isDetected");
                SetAutoPathFinding(waterSlimeanimator, waterSlimerigidbody);
            }
            else
            {
                waterSlimeanimator.SetTrigger("isDetected");
                if (waterSlimeanimator.GetBool("shouldStand"))
                {
                    SetStand(waterSlimeanimator, waterSlimerigidbody);
                } 
                else if (attackRange >= attackDistance || isFlying)
                {
                    flyDistance = attackDistance;
                    parabolicMotion(waterSlimeanimator, waterSlimerigidbody);
                }
            }
        }
    }
}
