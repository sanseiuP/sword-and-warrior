using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : Item
{
    public GameObject bulletPrefab;
    public float bulletForce;
    public Transform pos;
    public float CD = 1;
    public float timing = 0;

    public override void ShootButtonDown()
    {
        if (Time.time - timing >=CD)
        {
            timing = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, pos.position, pos.rotation*Quaternion.AngleAxis(Random.Range(0,shake),Vector3.forward));
            bullet.GetComponent<Bullet>().InitializationOfBullet(attack, role, bulletForce);
           
            //射击动画效果
            //GetComponent<Animator>().Play("Shoot");
        }
    }

    public override void UpdateLookAt(Vector3 target)
    {
        transform.right = (target - transform.position).normalized;
        if (transform.position.x > target.x)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (transform.position.x < target.x)
        {
           GetComponent<SpriteRenderer>().flipY = false;
        }
    }
}