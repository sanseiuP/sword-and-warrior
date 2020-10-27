using System;
using UnityEngine;

//对人物的基本操作
//暂时将所有函数都设置为public void
public class Command : MonoBehaviour
{
   
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
                player.setMove(0);
                break;
            case 0b0010:
            case 0b0111:
                player.setMove(-3.1415926f/2);
                break;
            case 0b0011:
                player.setMove(-3.1415926f/4);
                break;
            case 0b0100:
            case 0b1110:
                player.setMove(-3.1415926f);
                break;
            case 0b0110:
                player.setMove(-3.1415926f*3/4);
                break;
            case 0b1000:
            case 0b1101:
                player.setMove(3.1415926f/2);
                break;
            case 0b1001:
                player.setMove(3.1415926f/4);
                break;
            case 0b1100:
                player.setMove(3.1415926f*3/4);
                break;            
            default:
                break;
        }
    }

    public void roll(float direction)
    {

    }

    public void Attack(Actor player)
    {

    }

    public void switchWeapon()
    {

    }
    public void interact()
    {

    }



}