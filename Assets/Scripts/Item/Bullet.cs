using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    public GameObject hitEffect; //子弹附带攻击效果
    protected float attack; 
    protected string role;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.transform.tag != role && collision.transform.tag != "Bullet"&& collision.transform.tag != "Room"&& collision.transform.tag != "Weapon")
        if (collision.transform.tag != role)
        {
            if (collision.GetComponent<BeAttack>() != null)
            {
                collision.GetComponent<BeAttack>().BeAttack(attack);
                
            }
            else if (collision.transform.tag == "Wall")
            {
                
            }
        }

    }

    public override void InitializationOfBullet(float attack, string role, float bulletForce)
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * bulletForce, ForceMode2D.Impulse);
        this.attack = attack;
        this.role = role;
    }
}