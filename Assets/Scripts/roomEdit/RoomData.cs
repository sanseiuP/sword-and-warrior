using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//记录房间的信息
struct RoomInfo {
    Vector2Int size; //房间的大小，用width*height表示、
    //三层Tilemap
    Tilemap ground;
    Tilemap groundDetails;
    Tilemap onTheGround;
    //出口信息，四个方向
    Vector2Int [] topExits;
    Vector2Int [] leftExits;
    Vector2Int [] rightExits;
    Vector2Int [] BottomExits;
}

public class RoomData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

