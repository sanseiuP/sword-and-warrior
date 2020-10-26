using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemGenerator :MonoBehaviour
{
    private static ItemGenerator instance=null; //单例模式

    private ItemGenerator(){}

    public static ItemGenerator getInstance()
    {
        if (instance==null) instance=new ItemGenerator();
        return instance;
    }

    public Dictionary<string,Item> item_dict = new Dictionary<string,Item>();

    public void addItem(string name, Item i)
    {
        item_dict.Add(name,i);
    }

    public Item generate(string name)
    {
        return item_dict[name];
    }
}