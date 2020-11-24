using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Crystal : Item 
{
    public float speed = 2;
    public float getDistance = 5;
    Vector2 target;
    Transform player;
    bool isDrop;
    //这里应该加上水晶的功能相关的属性
    void Start()
    {
        target = transform.position + new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        player = GameManager.instance.troops[0].transform;
    }
    private void Update()
    {
        if (!isDrop)
        {
            if (Vector2.Distance(transform.position, target) > 0.01)
            {
                transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed);
            }
            else
            {
                isDrop = true;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) <= getDistance)
            {
                transform.position = Vector2.Lerp(transform.position, player.position, Time.deltaTime * speed);
            }
            if (Vector2.Distance(transform.position, player.position) <= 0.5)
            {
                //这里应该调用player的接口实现水晶功能
                Destroy(gameObject);
            }
        }
    }
}