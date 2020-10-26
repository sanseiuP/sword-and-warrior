using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{
    public int[,] ma;
    public List<V3> exit; 
    public void Start()
    {
        MapRandom M=new MapRandom();
        int inx=M.AddRoom(1);
        M.AddDoor(0, 0, 1);
        M.AddDoor(0, 0, 4); 
        M.AddDoor(0, 0, 7);
        M.AddDoor(0, 0, 10);

        inx = M.AddRoom(11);
        M.AddDoor(1, 0, 10);
        M.AddDoor(1, 3, 1);
       

        ma = M.GetMap(5,5,12);
        exit = M.GetExit();
      //  for (int i = 0; i < 5; i++)
        //    for (int j = 0; j < 5; j++) print(ma[i, j]);
    }

    
    void Update()
    {
        
    }
}
