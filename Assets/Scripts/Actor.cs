using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //人物数据相关
    private string name;
    private int totalHP, currentHP;
    public float speed = 3f;
    private bool isMoving = false;
    Vector2 moveDirection = new Vector2(1, 0);//移动方向，默认朝右

    //音频相关
    private AudioSource audioSource;
    public AudioClip Footsteps;

    Transform transform;//获取人物位置信息
    Rigidbody2D rigidbody;//人物刚体模型
    Animator animator;//控制动画相关
    Attack attack;
    public GameObject bulletObj;

    public void setMove(float direction)//设置移动，方向由弧度制表示
    {
        isMoving = true;
        moveDirection.x = (float)Math.Cos(direction);
        moveDirection.y = (float)Math.Sin(direction);
        //移动动画相关
        animator.SetBool("isMoving", true);
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        //移动音频相关
        if (!audioSource.isPlaying)
            audioSource.Play();
        Vector2 position = rigidbody.position;
        position += moveDirection * speed * Time.deltaTime;
        rigidbody.MovePosition(position);//使人物朝移动方向每帧移动speed的长度
    }

    public void setStand()//设置人物静置
    {
        animator.SetBool("isMoving", false);
        audioSource.Pause();
    }

    public void encounterAttack()//发出攻击
    {
        GameObject bullet = Instantiate(bulletObj, rigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
        Attack bulletController = bullet.GetComponent<Attack>();
        if (bulletController != null)
        {
            bulletController.Shoot(moveDirection);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //获取组件
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            isMoving = false;
        else
            setStand();
    }
}
