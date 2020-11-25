using System;
using UnityEngine;

public enum InteractEntityType
{
    Bullet=0,
    Coin=1,
    Crystal=2,
    Gun=3
};

//邹凯韬、张志扬负责
public class InteractEntity :MonoBehaviour
{
    public InteractEntityType type;
    public Item item;
    public int cost;

    InteractEntity(InteractEntityType t, Item i, int c){
        this.type=t;
        this.item=i;
        this.cost=c;
    }
}