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
    private ArrayList doors = new ArrayList(); //所有的门


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


    /* 在指定单元格处指定位置填充桥
     * i,j：单元序号；bridgeIndex：桥的索引；depth1：桥入口深度；depth2：桥出口的深度
     */
    private void paintBridge(int i, int j, int bridgeIndex, int depth1, int depth2) {
        //根据桥的具体位置指定填充位置
        int baseX = 0, baseY = 0;
        switch (bridgeIndex) {
            case 0: baseX = i*16 + 1; baseY = (j+1) * 16 - depth1; break;
            case 1: baseX = i*16 + 6; baseY = (j+1) * 16 - depth1; break;
            case 2: baseX = i*16 + 11; baseY = (j+1) * 16 - depth1; break;
            case 3: baseX = (i+1) * 16-depth1; baseY = j * 16 + 11; break;
            case 4: baseX = (i+1) * 16-depth1; baseY = j * 16 + 6; break;
            case 5: baseX = (i+1) * 16-depth1; baseY = j * 16 + 1; break;
            case 6: baseX = i*16 + 11; baseY = j * 16 - depth2; break;
            case 7: baseX = i*16 + 6; baseY = j * 16 - depth2; break;
            case 8: baseX = i*16 + 1; baseY = j * 16 - depth2; break;
            case 9: baseX = i*16 - depth2; baseY = j * 16 + 1; break;
            case 10: baseX = i*16 - depth2; baseY = j * 16 + 6; break;
            case 11: baseX = i*16 - depth2; baseY = j * 16 + 11; break;
		}


        //创建填充数组
        int count = (depth1+depth2)*4;
        TileBase[] bridgeBlocks = new TileBase[count];
        TileBase[] bridgeGround = new TileBase[count];
        TileBase[] bridgeinvisibleBlocks = new TileBase[count];

        //生成地面
        ArrayList.Repeat(tile_ground,bridgeGround.Length).CopyTo(bridgeGround);

        //分类：纵向桥和横向桥，生成桥沿和桥头
        int sizeX,sizeY;
        if (bridgeIndex <= 2 || (bridgeIndex >= 6 && bridgeIndex <= 8)) {
            sizeX = 4;
            sizeY = depth1 + depth2;

            //桥头桩
            bridgeBlocks[0] = tile_doorSide[2];
            bridgeBlocks[3] = tile_doorSide[3];
            bridgeBlocks[count - 4] = tile_doorSide[0];
            bridgeBlocks[count - 1] = tile_doorSide[1];
            //桥沿
            for (int k = 1; k < sizeY-1; k++)
                bridgeBlocks[k*4] = bridgeBlocks[k*4+3] = tile_block;
		
            //门
            GameObject temp;
            temp = GameObject.Instantiate<GameObject>(prefab_door_left);
            temp.GetComponent<Door>().x = baseX + 1;
            temp.GetComponent<Door>().y = baseY;
            doors.Add(temp);
            temp = GameObject.Instantiate<GameObject>(prefab_door_left);
            temp.GetComponent<Door>().x = baseX + 1;
            temp.GetComponent<Door>().y = baseY + sizeY - 1;
            doors.Add(temp);
            temp = GameObject.Instantiate<GameObject>(prefab_door_right);
            temp.GetComponent<Door>().x = baseX + 2;
            temp.GetComponent<Door>().y = baseY;
            doors.Add(temp);
            temp = GameObject.Instantiate<GameObject>(prefab_door_right);
            temp.GetComponent<Door>().x = baseX + 2;
            temp.GetComponent<Door>().y = baseY + sizeY - 1;
            doors.Add(temp);
            
        }
        else { 
            sizeX = depth1 + depth2;
            sizeY = 4;

            bridgeBlocks[0] = tile_doorSide[2];
            bridgeBlocks[sizeX-1] = tile_doorSide[3];
            bridgeBlocks[sizeX*3] = tile_doorSide[0];
            bridgeBlocks[count - 1] = tile_doorSide[1];
            for (int k = 1; k < sizeX-1; k++)
                bridgeBlocks[k] = bridgeBlocks[k+sizeX*3] = tile_block;

            //门
            GameObject temp;
            temp = GameObject.Instantiate<GameObject>(prefab_door_top);
            temp.GetComponent<Door>().x = baseX;
            temp.GetComponent<Door>().y = baseY + 2;
            doors.Add(temp);
            temp = GameObject.Instantiate<GameObject>(prefab_door_top);
            temp.GetComponent<Door>().x = baseX + sizeX - 1;
            temp.GetComponent<Door>().y = baseY + 2;
            doors.Add(temp);
            temp = GameObject.Instantiate<GameObject>(prefab_door_bottom);
            temp.GetComponent<Door>().x = baseX;
            temp.GetComponent<Door>().y = baseY + 1;
            doors.Add(temp);
            temp = GameObject.Instantiate<GameObject>(prefab_door_bottom);
            temp.GetComponent<Door>().x = baseX + sizeX - 1;
            temp.GetComponent<Door>().y = baseY + 1;
            doors.Add(temp);
		}


        //创建填充区域，填充tilemap
        BoundsInt region = new BoundsInt(baseX,baseY,0,sizeX,sizeY,1);
        map_ground.SetTilesBlock(region, bridgeGround);
        map_onTheGround.SetTilesBlock(region, bridgeBlocks);
        //用空数组清除不可见块
        map_invisibleBlocks.SetTilesBlock(region, bridgeinvisibleBlocks);
        
	}


    /* 清除地图
     */
    private void clearMap() {
        map_ground.ClearAllTiles();
        map_groundDetails.ClearAllTiles();
        map_onTheGround.ClearAllTiles();
	}

    /* 清除门
     */
    private void clearDoors() {
        foreach(GameObject ob in doors)
            DestroyImmediate(ob);
        doors.Clear();
	}


    /* 用于测试，在原点生成一个样例房间
     */
    public void generateSampleRoom()
    {
        if (allRoomsData.Length == 0)   {
            Debug.Log("No Room Data Found");
            return;
		}

        clearMap();
        clearDoors();

        int index = UnityEngine.Random.Range(0, allRoomsData.Length);

        paintRoom(0,0,allRoomsData[index]);
        mapSizeX = allRoomsData[index].sizeW * 16;
        mapSizeY = allRoomsData[index].sizeH * 16;

        for(int i=0; i < allRoomsData[index].bridgePositions.Length; i++)
            if (allRoomsData[index].bridgePositions[i] != -1)
                paintBridge((i/12)%allRoomsData[index].sizeW,(i/12)/allRoomsData[index].sizeW,
                i%12,allRoomsData[index].bridgePositions[i],UnityEngine.Random.Range(1,6));

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


    /*启动所有的门
     */
    private void startDoors() {
        foreach(GameObject ob in doors)
            ob.GetComponent<Door>().init();
	} 


    /*打开所有的门
     */
    public void openAllDoors() {
        foreach(GameObject ob in doors)
            ob.GetComponent<Door>().setOpen();
	}
    
    /*关闭所有的门
     */
    public void closeAllDoors() {
        foreach(GameObject ob in doors)
            ob.GetComponent<Door>().setClose();
	}


	private void Start()
	{
		setReferences();
        getRoomsData();
        generateSampleRoom();
        startDoors();
	}

	private void Update()
		{
		if (Input.GetKeyDown(KeyCode.O))
            openAllDoors();
        if (Input.GetKeyDown(KeyCode.C))
            closeAllDoors();
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