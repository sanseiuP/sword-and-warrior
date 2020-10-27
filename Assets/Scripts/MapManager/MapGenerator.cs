using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    /*游戏对象索引
     */
    /*对地图的索引*/
    private Tilemap map_ground;
    private Tilemap map_groundDetails;
    private Tilemap map_onTheGround;
    private Tilemap map_invisibleBlocks;

    /*tile的索引*/
    private TileBase[] tile_doorSide = new TileBase[4];
    private Tile tile_ground, tile_block, tile_invisibleBlock;

    /*prefab的索引*/
    private GameObject prefab_door_top, prefab_door_bottom, prefab_door_left, prefab_door_right;

    /*tile与ID的映射工具*/
    public Tile_ID_Convertion Convertor;


    /*资源数据
     */
    /*所有房间的数据*/
    private RoomData[] allRoomsData;


    /*辅助类
     */
    /*地图的布局工具*/
    private MapRandom mapLayouter;


    /*实时数据
     */
    private int mapSizeX, mapSizeY; //地图尺寸
    private int[] roomIndex; //房间在layout中的id
    private ArrayList doors; //所有的门


    /*加载所有外部引用
     */
    public void setReferences() {
        //获取tilemap每一层的索引
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_Ground")
                map_ground = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_GroundDetails")
                map_groundDetails = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_OnTheGround")
                map_onTheGround = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_InvisibleBlocks")
                map_invisibleBlocks = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		}

        //获取tile
        tile_doorSide[0] =  AssetDatabase.LoadAssetAtPath<Tile>
        ("Assets/Palettes/roomTiles/doors/doorBlocks_0.asset");
        tile_doorSide[1] =  AssetDatabase.LoadAssetAtPath<Tile>
        ("Assets/Palettes/roomTiles/doors/doorBlocks_1.asset");
        tile_doorSide[2] =  AssetDatabase.LoadAssetAtPath<Tile>
        ("Assets/Palettes/roomTiles/doors/doorBlocks_2.asset");
        tile_doorSide[3] =  AssetDatabase.LoadAssetAtPath<Tile>
        ("Assets/Palettes/roomTiles/doors/doorBlocks_3.asset");

        tile_ground = AssetDatabase.LoadAssetAtPath<Tile>
        ("Assets/Palettes/roomTiles/Rule tiles/mixed ground RandomTile.asset");
        tile_block = AssetDatabase.LoadAssetAtPath<Tile>
        ("Assets/Palettes/roomTiles/Rule tiles/may_broken block RandomTile.asset");
        tile_invisibleBlock = AssetDatabase.LoadAssetAtPath<Tile>
        ("Assets/Palettes/roomTiles/invisibleBlock.asset");

        //获取prefab
        prefab_door_top =  AssetDatabase.LoadAssetAtPath<GameObject>
        ("Assets/Prefab/doors/door_top.prefab");
        prefab_door_bottom =  AssetDatabase.LoadAssetAtPath<GameObject>
        ("Assets/Prefab/doors/door_bottom.prefab");
        prefab_door_left =  AssetDatabase.LoadAssetAtPath<GameObject>
        ("Assets/Prefab/doors/door_left.prefab");
        prefab_door_right =  AssetDatabase.LoadAssetAtPath<GameObject>
        ("Assets/Prefab/doors/door_right.prefab");
       
        prefab_door_top.GetComponent<Door>().map_invisibleBlocks = map_invisibleBlocks;
        prefab_door_bottom.GetComponent<Door>().map_invisibleBlocks = map_invisibleBlocks;
        prefab_door_left.GetComponent<Door>().map_invisibleBlocks = map_invisibleBlocks;
        prefab_door_right.GetComponent<Door>().map_invisibleBlocks = map_invisibleBlocks;
        
        prefab_door_top.GetComponent<Door>().tile_invisibleBlock = tile_invisibleBlock;
        prefab_door_bottom.GetComponent<Door>().tile_invisibleBlock = tile_invisibleBlock;
        prefab_door_left.GetComponent<Door>().tile_invisibleBlock = tile_invisibleBlock;
        prefab_door_right.GetComponent<Door>().tile_invisibleBlock = tile_invisibleBlock;

        //获取tile_ID映射工具
        Convertor = AssetDatabase.LoadAssetAtPath<Tile_ID_Convertion>
        ("Assets/Resourse/Tile_ID_Convertor.asset");
	}


    /* 加载房间数据并加载mapLayouter
     */
    public void getRoomsData() {
        //加载房间数据至数组
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.dataPath+"/Resourse/RoomData/AllRoomsData", FileMode.Open);
        allRoomsData = bf.Deserialize(fs) as RoomData[];
        fs.Close();

        //加载mapLayouter
        roomIndex = new int[allRoomsData.Length];
        mapLayouter = new MapRandom();
        for (int i = 0; i < allRoomsData.Length; i ++) {
            RoomData temp = allRoomsData[i];

            int roomLayout = 0b0_0000_0000;
            for (int m = 0; m < 9; m ++) 
                if (m % 3 < temp.sizeW && m/3 < temp.sizeH)
                    roomLayout = roomLayout | (1 << m);
            roomIndex[i] = mapLayouter.AddRoom(roomLayout);
            Debug.Log("mapLayouter.AddRoom(" + roomLayout +")");

            for (int j = 0; j < temp.sizeH; j++)
                for (int k = 0; k < temp.sizeW; k++)
                    for (int m=0; m<12; m++)
                        if (temp.bridgePositions[(j*temp.sizeW+k)*12+m] != -1) { 
                            mapLayouter.AddDoor(roomIndex[i], j*3+k, m);
                            Debug.Log("mapLayouter.AddDoor("+roomIndex[i]+", "+(j*3+k)+", "+m+")");
                            }
		}
	}


    /* 在指定坐标处填充房间数据
     */
    private void paintRoom(int x, int y, RoomData data) {
        //创建透明碰撞方块
        TileBase[] data_invisibleBlock = new TileBase[data.ground.Length];
        for (int i = 0; i < data.ground.Length; i ++)
            if (data.ground[i] == -1 && data.onTheGround[i] == -1)
                data_invisibleBlock[i] = tile_invisibleBlock;

        //拷贝数据到当前地图中
        BoundsInt region = new BoundsInt(x,y,0,data.sizeW*16, data.sizeH*16,1);
        map_ground.SetTilesBlock(region,Convertor.fromIDsToTiles(data.ground));
        map_groundDetails.SetTilesBlock(region,Convertor.fromIDsToTiles(data.groundDetails));
        map_onTheGround.SetTilesBlock(region,Convertor.fromIDsToTiles(data.onTheGround));
        map_invisibleBlocks.SetTilesBlock(region,data_invisibleBlock);
	}


    /* 清除地图
     */
    private void clearMap() {
        map_ground.ClearAllTiles();
        map_groundDetails.ClearAllTiles();
        map_onTheGround.ClearAllTiles();
	}


    /* 用于测试，在原点生成一个样例房间
     */
    public void generateSampleRoom()
    {
        if (allRoomsData == null)   getRoomsData();
        if (allRoomsData.Length == 0)   {
            Debug.Log("No Room Data Found");
            return;
		}
        if (map_ground == null) setReferences();

        clearMap();
        int index = UnityEngine.Random.Range(0, allRoomsData.Length);
        paintRoom(0,0,allRoomsData[index]);

        mapSizeX = allRoomsData[index].sizeW * 16;
        mapSizeY = allRoomsData[index].sizeH * 16;
	}


    /*在指定范围内生成一张地图
     */
     public void generateMapInArea(int width, int height, int area) {
        clearMap();
        
        int[,] mapLayout = mapLayouter.GetMap(width, height, area);
        for (int i = 0; i < width; i++) 
            for (int j = 0; j < height; j++) 
                if(mapLayout[i,j] != -1) {
                    paintRoom(i*16,j*16,allRoomsData[roomIndex[mapLayout[i,j] ] ]);
				}
	 }


	private void Start()
	{
		setReferences();
        getRoomsData();
	}

}


[CustomEditor(typeof(MapGenerator))]
public class CustomMapGenerator : Editor
{
    MapGenerator m_target;    

	public override void OnInspectorGUI() {
        m_target = target as MapGenerator;

		base.OnInspectorGUI();

        if (GUILayout.Button("获取引用")) {
            m_target.setReferences();
		}
        
        if (GUILayout.Button("读取数据")) {
            m_target.getRoomsData();
		}
        
        if (GUILayout.Button("生成单个房间")) {
            m_target.generateSampleRoom();
		}

        if (GUILayout.Button("生成地图")) {
            m_target.generateMapInArea(5,5,9);
		}
	}
}