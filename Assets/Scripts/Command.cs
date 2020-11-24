using System;
using UnityEngine;

//对人物的基本操作
//暂时将所有函数都设置为public void
public class Command : MonoBehaviour
{
    GameObject[] obj;

    void Awake() //保证obj在Command生成时就被初始化
    {
        obj = FindObjectsOfType(typeof(GameObject)) as GameObject[]; //获取所有gameobject元素给数组obj
    }
   
    public void move(Actor player) //不用传进移动方向direction，由getMoveInput()函数逐帧获取move_direction即可移动
    {
        int move_direction=InputManager.getInstance().getMoveInput();
        switch (move_direction)
        {
            case 0b0000:
            case 0b0101:
            case 0b1010:
            case 0b1111:
                break;
            case 0b0001:
            case 0b1011:
                player.SetMove(0);
                break;
            case 0b0010:
            case 0b0111:
                player.SetMove(-3.1415926f/2);
                break;
            case 0b0011:
                player.SetMove(-3.1415926f/4);
                break;
            case 0b0100:
            case 0b1110:
                player.SetMove(-3.1415926f);
                break;
            case 0b0110:
                player.SetMove(-3.1415926f*3/4);
                break;
            case 0b1000:
            case 0b1101:
                player.SetMove(3.1415926f/2);
                break;
            case 0b1001:
                player.SetMove(3.1415926f/4);
                break;
            case 0b1100:
                player.SetMove(3.1415926f*3/4);
                break;            
            default:
                break;
        }
    }

    public void roll(float direction)
    {
        //what is roll? The roll of the warrior?
    }

    public void Attack(Actor player)
    {
        float min_distance=-1.0;
        Enemy e=null;
        foreach (GameObject child in obj) //遍历所有gameobject
        {
            //Debug.Log(child.gameObject.name); //可以在unity控制台测试一下是否成功获取所有元素
            if (child.gameObject.tag == "Enemy") //如果是Enemy，就计算和player的距离
            {
                float distance;
                Vector2 temp;
                temp.x=child.gameObject.rigidbody.position.x-player.rigidbody.position.x;
                temp.y=child.gameObject.rigidbody.position.y-player.rigidbody.position.y;
                distance=Math.Sqrt(temp.x*temp.x+temp.y*temp.y);
                if (e==null||distance<min_distance) {
                    e=child.gameObject;
                    min_distance=distance;
                }
                Destroy(child.gameObject);
            }
        }
        Vector2 direction;
        direction.x=e.rigidbody.position.x-player.rigidbody.position.x;
        direction.y=e.rigidbody.position.y-player.rigidbody.position.y;
        player.attack.Shoot(direction);
        player.attack.OnCollisionEnter2D(e);
    }

    public void switchWeapon(Warrior w, float threshold=0.5) //threshold为捡枪最小距离
    {
        float min_distance=-1.0;
        Item weapon=null;
        foreach (GameObject child in obj) //遍历所有gameobject
        {
            if (child.gameObject.tag == "Item") //如果是Item，就计算和w的距离
            {
                float distance;
                Vector2 temp;
                temp.x=child.gameObject.rigidbody.position.x-w.rigidbody.position.x;
                temp.y=child.gameObject.rigidbody.position.y-w.rigidbody.position.y;
                distance=Math.Sqrt(temp.x*temp.x+temp.y*temp.y);
                if (weapon==null||distance<min_distance) {
                    weapon=child.gameObject;
                    min_distance=distance;
                }
                Destroy(child.gameObject);
            }
        }
        if (min_distance>=0&&min_distance<=threshold) {
            w.setWeapon(weapon);
        }
    }

    public void interact()
    {
        
    }



}