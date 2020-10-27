using System;
using UnityEngine;

//邹凯韬、张志扬负责

//监听用户输入
public class InputManager : MonoBehaviour
{
    private static InputManager instance=null; //单例模式

    private InputManager(){}

    public static InputManager getInstance() //通过getInstance()函数访问实例
    {
        if (instance==null) instance=new InputManager();
        return instance;
    }

    public int getMoveInput()
    {
        int ismove=0b0000;
        if (Input.GetKey(KeyCode.W)) ismove=ismove^0b1000;
        if (Input.GetKey(KeyCode.A)) ismove=ismove^0b0100;
        if (Input.GetKey(KeyCode.S)) ismove=ismove^0b0010;
        if (Input.GetKey(KeyCode.D)) ismove=ismove^0b0001;
        return ismove;
    }

    public bool getAttackInput()
    {
        return Input.GetKey(KeyCode.F)||Input.GetKeyDown(KeyCode.F);
    }
}