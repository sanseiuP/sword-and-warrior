using System.Collections.Generic;
using System;
using UnityEngine;

public class InteractEntityGenerator :MonoBehaviour
{
    //这里应该是用dictionary？
    public Dictionary<string,InteractEntity> prototype = new Dictionary<string,InteractEntity>();

    public void addPrototype(string name, InteractEntity pt)
    {

    }

    public void generate(string name)
    {
        
    }
}