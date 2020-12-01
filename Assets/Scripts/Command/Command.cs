using System;
using UnityEngine;
using Unity.Entities;


//指令的基类，也是被观察者
public abstract class Command
{
    public string name; //命令的名称
    public Warrior ob; //命令所作用的对象
    private CommandManager manager; //指向父对象,但是私有，子类只能以观察者模式与其耦合

    public Command(string name_) {
        name = name_;
	}

    //指令的执行，返回值为true表示这条指令执行完毕，否则表示还需要执行
    public abstract bool execute();

    //设置CommendManager的引用，由CommendManager调用
    public void setReference(CommandManager manager_, Warrior ob_) {
        manager = manager_;
        ob = ob_;
	}

    //观察者模式，用于子类与CommandManager的解耦
    protected void notify(string message) {
        manager.notified(message);
	}
}


//用于移动的命令
public class Command_Move : Command
{
    //移动的角度
    public float angle; 

    public Command_Move(float angle_) : base("Move")
    {
        angle = angle_;
	}

    override
    public bool execute() {
		ob.SetMove(angle);
        return true;
	}
}

//强制移动至某一位置(附近）的命令，执行期间此对象上其它的命令将被忽略
public class Command_MoveTo : Command
{
    public float dx, dy; //移动的目的地
    public Command_MoveTo(float dx_, float dy_) : base("MoveTo")
    {
        dx = dx_;
        dy = dy_;
	}

    override
    public bool execute() {
        //首先向CommandManager发送禁用玩家操作的消息，可能会重复调用
        notify("ForbidPlayerControl");

        return false;
	}
}

//用于攻击的命令
public class Command_Wave : Command
{
    public Command_Wave() : base("Wave")
    { }

    override
    public bool execute() {
        ob.setWave();

        return true;
    }
}