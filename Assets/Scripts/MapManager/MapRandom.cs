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

public struct V6
{
    public int room1, block1, door1,
        room2, block2, door2;

}


public class MapRandom
{
  

    public List<Room> rooms = new List<Room>();
    public node[,] map = new node[100, 100];
    public List<V6> exitPair = new List<V6>();

    int roomNum = 0;
    int length = 0;
    int width = 0;
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


   private void Init(node[] a)
    {
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
   


    private bool Check(int idRoom, node p)
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
            if (nx < 0 || nx >= length || ny < 0 || ny >= width || map[nx, ny].x > 0) return false;            

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
                if (key1 != key2) return false;
                if (key1 > 0) suc = true;

            }

        }

        return suc;


    }

    private void Put_Room(List<V6> exitPair,int[,] roomPos,int idRoom, node p)
    {
        Room room = rooms[idRoom];

        int[] door = new int[6];
        int px = p.x;
        int py = p.y;

        foreach (int id in room.blocks.Keys)
        {
            Block block = room.blocks[id];
            int nx = px + id % 3;
            int ny = py + id / 3;
           

            if (id == 0) roomPos[nx,ny] = idRoom;

            for (int i = 0; i < 4; i++)
            {
                int dx = nx + direction[i,0];
                int dy = ny + direction[i,1];
                if (dx < 0 || dx >= length || dy < 0 || dy >= width || map[dx,dy].x == -1) continue;

                int otherRoomId = map[dx,dy].x;
                int otherBoardId = map[dx,dy].y;
                Block otherBlock = rooms[otherRoomId].blocks[otherBoardId];
                int key = block.DoorKey(i);
                for (int j = 0; j <= 5; j++) door[j] = -1;

                if (i == 0)
                {
                    if ((key >> 2) == 1) {
                        door[0] = 0;
                        door[1] = 8;
                    }
                    if (((key >> 1) & 1) == 1) {
                        door[2] = 1;
                        door[3] = 7;
                    }
                    if ((key & 1) == 1)
                    {
                        door[4] = 2;
                        door[5] = 6;
                    }

                }else if (i==1)
                {
                    if ((key >> 2) == 1)
                    {
                        door[0] = 8;
                        door[1] = 0;
                    }
                    if (((key >> 1) & 1) == 1)
                    {
                        door[2] = 7;
                        door[3] = 1;
                    }
                    if ((key & 1) == 1)
                    {
                        door[4] = 6;
                        door[5] = 2;
                    }

                }
                else if (i == 2)
                {
                    if ((key >> 2) == 1)
                    {
                        door[0] = 3;
                        door[1] = 11;
                    }
                    if (((key >> 1) & 1) == 1)
                    {
                        door[2] = 4;
                        door[3] = 10;
                    }
                    if ((key & 1) == 1)
                    {
                        door[4] = 5;
                        door[5] = 9;
                    }

                }
                else if (i == 3)
                {
                    if ((key >> 2) == 1)
                    {
                        door[0] = 11;
                        door[1] = 3;
                    }
                    if (((key >> 1) & 1) == 1)
                    {
                        door[2] = 10;
                        door[3] = 4;
                    }
                    if ((key & 1) == 1)
                    {
                        door[4] = 9;
                        door[5] = 5;
                    }

                }
                 
                for(int j = 0; j < 5; j += 2) if (door[j] >= 0 && door[j + 1] >= 0)
                    {
                        V6 ans;
                        ans.room1 = idRoom;
                        ans.room2 = otherRoomId;
                        ans.block1 = id;
                        ans.block2 = otherBoardId;
                        ans.door1 = door[j];
                        ans.door2 = door[j + 1];
                        exitPair.Add(ans);
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



    public int [,] GetMap(int len,int wid,int area)//，在长，宽范围内生成大地图，返回二维数组，0为空，表示房间信息。
    {
        exitPair.Clear();
        length = len;
        width = wid;
        int totArea = length * width;      
        int[,] roomPos = new int[length, width];
        node[] pos = new node[totArea];        

        Init(pos);
        Rand_pos(pos, totArea);


        int areaNow = 0;
        while (areaNow <= area)
        {
            int idRoom = UnityEngine.Random.Range(0, roomNum);
            Rand_pos(pos, totArea);

            for(int i = 0; i < totArea; i++)
            {
                if (Check(idRoom, pos[i])) 
                {
                    Put_Room(exitPair,roomPos,idRoom, pos[i]);
                    areaNow += rooms[idRoom].area;
                }
            }

        }

        return roomPos;
               
    }

    public List<V6> GetExitPair()
    {
        return exitPair;
    }
    

   

}
