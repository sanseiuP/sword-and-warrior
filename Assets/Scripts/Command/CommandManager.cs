using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    List<Command> commands = new List<Command>(); //指令流

    public Warrior player; //玩家的引用

    bool player_acceptControl = true; //标记玩家是否接受输入


    //添加一个指令到指令流中
    public void addCommand(Command command) {
        //当玩家不接受操纵时，丢弃对玩家的操作命令
        if (player_acceptControl == false && command.ob.gameObject.tag == "Player" )
            return;

        //当上述条件都不满足时，添加指令
        command.setReference(this, player);
        commands.Add(command);
	}


    //用于接收消息
    public void notified(string message)  {
        if (message == "ForbidPlayerControl")
            player_acceptControl = false;
        else if (message == "EnablePlayerControl")
            player_acceptControl = true;
	}


    //固定时间执行
	private void Update() {
            //执行所有的指令
		    for(int i = 0; i < commands.Count; i ++) {
                if(commands[i].execute() == true)
                    commands.RemoveAt(i); //当execute返回true时移除commend
		    }
		}
	}
