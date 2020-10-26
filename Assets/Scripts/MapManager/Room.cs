using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    int model = 0;
    int index = 0;
    public int area = 0;

    public Dictionary<int,Block> blocks = new Dictionary<int, Block>();

    public Room(int mod,int ind)
    {
        index = ind;
        model = mod;

        int cnt = 0;
        while (mod>0)
        {
            if ((mod & 1) == 1)
            {
                blocks.Add(cnt, new Block(cnt));
                area++;
            }
            cnt++;
            mod >>= 1;
        }

    }

   

    public void AddDoor(int blockIndex,int doorId)
    {
        blocks[blockIndex].AddDoor(doorId);
    }
}
