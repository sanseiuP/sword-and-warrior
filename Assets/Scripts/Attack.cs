using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Transform transform;
    private float speed = 500;
    private float life = 2f;
    Rigidbody2D rigidbody;//子弹/剑的刚体模型

    void OnCollisionEnter2D(Collision2D other)//攻击与外界的碰撞检测
    {
        Actor actor = other.gameObject.GetComponent<Actor>();//获取与之碰撞的对象
        Destroy(this.gameObject);//无论是否为敌人，产生碰撞后销毁
        if (actor != null)//与敌人产生碰撞
        {

        }
    }

    public void Shoot(Vector2 shootDirection)
    {
        rigidbody.AddForce(shootDirection * speed);
    }

    // Start is called before the first frame update
    void Awake()//使用Awake保证攻击对象一被生成就进行初始化
    {
        rigidbody = GetComponent<Rigidbody2D>();//获取组件
        Destroy(this.gameObject, life);//在指定时间后自动销毁
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
