using System;
using UnityEngine;


public class InputManager : MonoBehaviour
{
public CommandManager commandManager;

const float PI = 3.1415926f;

private void Update() {
		//获取移动
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		if (v > 0) { 
			if (h > 0) commandManager.addCommand(new Command_Move(PI/4.0f));
			else if (h == 0) commandManager.addCommand(new Command_Move(PI/2.0f));
			else if (h < 0) commandManager.addCommand(new Command_Move(PI/4.0f*3.0f));
		}
		else if (v < 0) { 
			if (h > 0) commandManager.addCommand(new Command_Move(-PI/4.0f));
			else if (h == 0) commandManager.addCommand(new Command_Move(-PI/2.0f));
			else if (h < 0) commandManager.addCommand(new Command_Move(-PI/4.0f*3.0f));
		}
		else if (v == 0) { 
			if (h > 0) commandManager.addCommand(new Command_Move(0.0f));
			else if (h < 0) commandManager.addCommand(new Command_Move(PI));
			//else Debug.Log("STOP");
		}

		//获取攻击
		if (Input.GetAxis("Fire1") != 0)
			commandManager.addCommand(new Command_Wave());
	}
}