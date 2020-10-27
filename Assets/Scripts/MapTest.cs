using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
    public node[,] ma;
    public List<V3> exit; 
    public void Start()
    {
        MapRandom M=new MapRandom();
        int inx=M.AddRoom(1);
        M.AddDoor(0, 0, 1);
        M.AddDoor(0, 0, 2); 
        M.AddDoor(0, 0, 4);
        M.AddDoor(0, 0, 6);
        M.AddDoor(0, 0, 7);
        M.AddDoor(0, 0, 10);

        inx = M.AddRoom(3);
        M.AddDoor(1, 0, 1);
        M.AddDoor(1, 0, 7);
        M.AddDoor(1, 0, 10);
        M.AddDoor(1, 1, 1);
        M.AddDoor(1, 1, 5);
        M.AddDoor(1, 1, 7);
        M.AddDoor(1, 1, 8);

        inx = M.AddRoom(9);
        M.AddDoor(2, 0, 3);
        M.AddDoor(2, 0, 4);
        M.AddDoor(2, 0, 5);
        M.AddDoor(2, 0, 7);
        M.AddDoor(2, 0, 9);
        M.AddDoor(2, 0, 11);
        M.AddDoor(2, 3, 1);
        M.AddDoor(2, 3, 4);
        M.AddDoor(2, 3, 10);        
        ma = M.GetMap(5,5,12);
        exit = M.GetExit();
      //  for (int i = 0; i < 5; i++)
        //    for (int j = 0; j < 5; j++) print(ma[i, j]);
    }

    
    void Update()
    {
        
    }
}
