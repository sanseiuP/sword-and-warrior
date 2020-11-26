using System;
using UnityEngine;
using Unity.Entities;

//对人物的基本操作
//暂时将所有函数都设置为public void
public class Command : MonoBehaviour
{
    //GameObject[] obj;

    void Awake() //保证obj在Command生成时就被初始化
    {
        //obj = FindObjectsOfType(typeof(GameObject)) as GameObject[]; //获取所有gameobject元素给数组obj
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

    
    public void roll(double direction)
    {
        //what is roll? The roll of the warrior?
    }

    public void Attack(Actor player)
    {
        //如果input_manager检测到按下F键，就执行此函数
        bool Is_Attacking=InputManager.getInstance().getAttackInput();
        if (Is_Attacking) {
            player.setWave();
        }
    }
    
    public void interact()
    {
        
    }
    
    public void SwapWeapon(Warrior warrior, Item weapon)
    {
        bool Is_Swap_Weapon=InputManager.getInstance().getSwapWeaponInput();
        if (Is_Swap_Weapon) {
            warrior.setWeapon(weapon);
        }
    }


}