using System;
using UnityEngine;
using Unity.Entities;


//ָ��Ļ��࣬Ҳ�Ǳ��۲���
public abstract class Command
{
    public string name; //���������
    public Warrior ob; //���������õĶ���
    private CommandManager manager; //ָ�򸸶���,����˽�У�����ֻ���Թ۲���ģʽ�������

    public Command(string name_) {
        name = name_;
	}

    //ָ���ִ�У�����ֵΪtrue��ʾ����ָ��ִ����ϣ������ʾ����Ҫִ��
    public abstract bool execute();

    //����CommendManager�����ã���CommendManager����
    public void setReference(CommandManager manager_, Warrior ob_) {
        manager = manager_;
        ob = ob_;
	}

    //�۲���ģʽ������������CommandManager�Ľ���
    protected void notify(string message) {
        manager.notified(message);
	}
}


//�����ƶ�������
public class Command_Move : Command
{
    //�ƶ��ĽǶ�
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

//ǿ���ƶ���ĳһλ��(�����������ִ���ڼ�˶��������������������
public class Command_MoveTo : Command
{
    public float dx, dy; //�ƶ���Ŀ�ĵ�
    public Command_MoveTo(float dx_, float dy_) : base("MoveTo")
    {
        dx = dx_;
        dy = dy_;
	}

    override
    public bool execute() {
        //������CommandManager���ͽ�����Ҳ�������Ϣ�����ܻ��ظ�����
        notify("ForbidPlayerControl");

        return false;
	}
}

//���ڹ���������
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