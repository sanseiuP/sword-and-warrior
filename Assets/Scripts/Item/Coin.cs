using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public float speed = 2; //硬币移速
    public float getDistance = 5;
    Vector2 target; //目标位置（硬币掉落后还移动一小段距离，位移终点称为目标位置）
    Transform player;
    bool isDrop;
    int value;
    
    Coin(int value=1,Transform player=null)
    {
        this.value=value;
        this.player=player;
    }

    void Start()
    {
        target = transform.position + new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0); //随机初始化目标位置
        isDrop = false;
        //player = GameManager.instance.troops[0].transform;
    }
    private void Update()
    {
        if (player==null) return; //硬币没有归属者，啥也不做
        if (!isDrop) //硬币还没到达目标位置
        {
            if (Vector2.Distance(transform.position, target) > 0.01) //硬币位置与目标位置距离还大于0.01
            {
                transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime * speed); //朝目标位置移动一小段距离
            }
            else //硬币位置与目标位置距离小于或等于0.01
            {
                isDrop = true; //硬币已经到达目标位置附近
            }
        }
        else //硬币到达目标位置附近
        {
            if (Vector2.Distance(transform.position, player.position) <= getDistance) //硬币距离和玩家（归属者）距离小于设定的距离
            {
                transform.position = Vector2.Lerp(transform.position, player.position, Time.deltaTime * speed); //硬币向玩家移动一小段距离
            }
            if (Vector2.Distance(transform.position, player.position) <= 0.5) //硬币距离和玩家（归属者）距离更小，小于0.5时
            {
                player.GetComponent<Warrior>().changeMoney(value); //玩家加钱
                Destroy(gameObject); //销毁硬币对象
            }
        }
    }
    
}
