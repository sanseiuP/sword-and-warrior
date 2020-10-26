using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block 
{
    int doorsNum = 0;
    int index = 0;
    int[] doors = new int[12];


    public void AddDoor(int doorId)
    {        
        doors[doorId] = 1;
        doorsNum++;          
    }

    public Block(int ind)
    {
        index = ind;
        for (int i = 0; i < 12; i++) doors[i] = 0;
    }

    public int DoorKey(int direction)
    {
        int key = 0;
        if (direction==0) key = doors[0] * 4 + doors[1] * 2 + doors[2];else
        if (direction==1) key = doors[8] * 4 + doors[7] * 2 + doors[6];else
        if (direction==2) key = doors[3] * 4 + doors[4] * 2 + doors[5];else
        if (direction==3) key = doors[11] * 4 + doors[10] * 2 + doors[9];
        return key;
    }

}
