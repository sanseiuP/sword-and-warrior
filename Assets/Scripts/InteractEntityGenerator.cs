using System.Collections.Generic;
using System;
using UnityEngine;

//邹凯韬、张志扬负责
public class InteractEntityGenerator :MonoBehaviour
{
    private static InteractEntityGenerator instance=null; //单例模式

    private InteractEntityGenerator(){}

    public static InteractEntityGenerator getInstance()
    {
        if (instance==null) instance=new InteractEntityGenerator();
        return instance;
    }

    public Dictionary<string,InteractEntity> prototype = new Dictionary<string,InteractEntity>();

    public void addPrototype(string name, InteractEntity pt)
    {
        prototype.Add(name,pt);
    }

    public InteractEntity generate(string name)
    {
        return prototype[name];
    }
}