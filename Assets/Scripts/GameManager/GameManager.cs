using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    /*需要创建整个场景的大地图，并绑定MapGenerator*/
    public GameObject map;

    /*需要创建人物*/
    public GameObject warrior;

    /*摄像机*/
    public GameObject cam;

    /*用于生成房间的参数*/
    public int sizeW , sizeH, area;

    /*房间的状态数据*/
    public enum RoomState { Unexplored, Conquered, Conquering }; //未探索的，已被征服，征服中
    public RoomState[,] roomState;

    // Start is called before the first frame update
    void Start()
    {
       regenerate();
    }

    void regenerate()
    {
        map.GetComponent<MapGenerator>().generateMapInArea(sizeW, sizeH, area);
        roomState = new RoomState [sizeW, sizeH];
        for(int i = 0; i < sizeW; i ++)
            for(int j = 0; j < sizeH; j ++)
                roomState[i,j] = RoomState.Unexplored;

        Vector2Int pos = map.GetComponent<MapGenerator>().getRandomPosition();
        warrior.transform.position = new Vector3(pos.x, pos.y);
        cam.GetComponent<SetBackground>().init(0, sizeW*16, sizeH*16, 0);
        timer = 2;
    }

    float timer = 2;
    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer--;
            AstarPathEditor.MenuScan();
        }
        if (Input.GetKeyDown(KeyCode.R))
            regenerate();
    }

    public void notified(string message, string parameter = "")
    {
        if (message.Equals("PlayerEnterRoom")) {
            //获取玩家位置
            int i = (int)warrior.transform.position.x / 16;
            int j = (int)warrior.transform.position.y / 16;
            //如果进入了一个未探索的房间
            if (roomState[i,j] == RoomState.Unexplored) {
                //获取当前玩家所在的房间区间
                int[] roomRegion = map.GetComponent<MapGenerator>().getRoomRegion(i,j);
                //调整这片区域的状态
                for(int u = 0; u < roomRegion[2]; u++)
                    for(int v = 0; v < roomRegion[3]; v++) 
                        roomState[u+roomRegion[0],v+roomRegion[1]] = RoomState.Conquering;

                //所有的门关闭
                map.GetComponent<MapGenerator>().closeAllDoors();
                Debug.Log("Enter [" + roomRegion[0] + " , " + roomRegion[1] + " ]");
			}
		}
	}

}
