using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

 public struct node
    {
        public int x, y;
        public void SetValue(int i,int j)
        {
            x = i;
            y = j;
        }
        static public void swap(ref node a,ref node b)
        {
            node temp = a;
            a = b;
            b = temp;
        }
    }

public struct V3
{
    public int x, y, no;

}


public class MapRandom
{
  

    public List<Room> rooms = new List<Room>();
    
    public List<V3> exit = new List<V3>();

    int detime = 0;
    int roomNum = 0;
    int length = 0;
    int width = 0;
    int nowRoomNum = 0;
    int[,] direction = new int[4, 2] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

    public int AddRoom(int modleState)//  添加房间，modleState:房间形状，返回房间索引
    {        
        rooms.Add(new Room(modleState,roomNum));
        return roomNum++;

    }

   public void AddDoor(int roomIndex,int blockIndex,int doorId)//为房间某块添加门
   {
        rooms[roomIndex].AddDoor(blockIndex,doorId);
     

   }


   private void Init(node[] a,node [,] map)
    {
        nowRoomNum = 0;
        for (int i = 0; i < length; i++)
            for (int j = 0; j < width; j++)
            {
                a[i * width + j].SetValue(i, j);
                map[i, j].x = -1;
                map[i, j].y = -1;               
            }

    }

    private void Rand_pos(node[] a, int len)
    {       
        for (int i = 0; i < len; i++)
        {
            int j = UnityEngine.Random.Range(0,len);
            node.swap(ref a[i], ref a[j]);
        }


    }
   


    private bool Check(int idRoom, node[,] map,node p)
    {      
        bool suc = false;
        int px = p.x;
        int py = p.y;
        Room room = rooms[idRoom];

        foreach (int id in room.blocks.Keys)
        {
            Block block = room.blocks[id];

            int nx = px + id % 3;
            int ny = py + id / 3;
            if (nx < 0 || nx >= length || ny < 0 || ny >= width || map[nx, ny].x >= 0) return false;            

            for (int i = 0; i < 4; i++)
            {
                int dx = nx + direction[i,0];
                int dy = ny + direction[i,1];
                if (dx < 0 || dx >= length || dy < 0 || dy >= width || map[dx,dy].x == -1) continue;

                int otherRoomId = map[dx,dy].x;
                int otherBoardId = map[dx,dy].y;
                Block otherBlock = rooms[otherRoomId].blocks[otherBoardId];
               
                int key1 = block.DoorKey(i);
                int key2 = otherBlock.DoorKey(i^1);
                if ((key1 & key2)==0) return false;

                if (detime <= 1000 && idRoom == otherRoomId) return false;

                if (key1 > 0) suc = true;

            }

        }
        if (nowRoomNum == 0) suc = true;
        return suc;


    }

    private void Put_Room(List<V3> exit,node[,] map,int idRoom, node p)
    {
        Room room = rooms[idRoom];

        int[] door = new int[3];
        int xx = 0;
        int yy = 0;


        int px = p.x;
        int py = p.y;

        foreach (int id in room.blocks.Keys)
        {
            Block block = room.blocks[id];
            int nx = px + id % 3;
            int ny = py + id / 3;
                    
            for (int i = 0; i < 4; i++)
            {
                int dx = nx + direction[i,0];
                int dy = ny + direction[i,1];
                if (dx < 0 || dx >= length || dy < 0 || dy >= width || map[dx,dy].x == -1) continue;

                int otherRoomId = map[dx,dy].x;
                int otherBoardId = map[dx,dy].y;
                Block otherBlock = rooms[otherRoomId].blocks[otherBoardId];
                int key = block.DoorKey(i);
                int key2 = otherBlock.DoorKey(i^1);
                for (int j = 0; j <= 2; j++) door[j] = -1;

                if (i == 0 || i == 1) 
                {
                    if (((key >> 2) == 1) && ((key2 >> 2) == 1)) door[0] = 2;
                  
                    if ((((key >> 1) & 1) == 1) && (((key2 >> 1) & 1) == 1)) door[1] = 1;
                   
                    if (((key & 1) == 1) && ((key2 & 1) == 1)) door[2] = 0;

                }
                else if (i == 2 || i == 3) 
                {
                    if (((key >> 2) == 1) && ((key2 >> 2) == 1)) door[0] = 3;

                    if ((((key >> 1) & 1) == 1) && (((key2 >> 1) & 1) == 1)) door[1] = 4;

                    if (((key & 1) == 1) && ((key2 & 1) == 1)) door[2] = 5;

                }

                if (i == 0 || i == 3) {
                    xx = nx;
                    yy = ny;
                 
                }else if (i == 1 || i == 2)
                {
                    xx = dx;
                    yy = dy;
                
                }


                for(int j = 0; j < 3; j++) if (door[j] >= 0)
                    {
                        V3 ans;
                        ans.x = xx;
                        ans.y = yy;                  
                        ans.no = door[j];
                        exit.Add(ans);                     
                    }



            }

        }

        foreach (int id in room.blocks.Keys)
        {
            Block block = room.blocks[id];
            int nx = px + id % 3;
            int ny = py + id / 3;
            map[nx,ny].x = idRoom;
            map[nx,ny].y = id;
        }
    }



    public node [,] GetMap(int len,int wid,int area)//，在长，宽范围内生成大地图，返回二维数组，0为空，表示房间信息。
    {
        exit.Clear();
        length = len;
        width = wid;
        int totArea = length * width;
        node[,] map = new node[length, width];
        node[] pos = new node[totArea];
        detime = 0;


        Init(pos,map);
        Rand_pos(pos, totArea);


        int areaNow = 0;     
        while (areaNow < area)
        {
           
            int idRoom = UnityEngine.Random.Range(0, roomNum);
            Rand_pos(pos, totArea);
            detime++;

            for(int i = 0; i < totArea; i++)
            {
                if (Check(idRoom, map, pos[i])) 
                {
                    nowRoomNum++;
                    Put_Room(exit,map,idRoom, pos[i]);
                    areaNow += rooms[idRoom].area;
                    break;
                    
                }
            }

        }

        return map;
               
    }

    public List<V3> GetExit()
    {
        return exit;
    }
    

   

}
