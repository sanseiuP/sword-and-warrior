using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[System.Serializable]
public struct RoomData {
    /*房间的大小，每16*16作为一个单位区域，可以占有最多3*3区域面积*/
    public int sizeW, sizeH;
    /*三层Tilemap*/
    public int[] ground;
    public int[] groundDetails;
    public int[] onTheGround;
    /*桥的位置，每个单位区域都可以选择12个桥的位点，对所有区域的所有位点的自由分量序列化
    * 区域以行主序，位点从最左侧top出口开始顺时针编号，得到单元(i,j)的第m号出口索引为
    * (i*sizeW + j) * 12 + m */
    public int[]  bridgePositions;

    public RoomData(int w, int h) {
        sizeW = w;
        sizeH = h;
        ground = groundDetails = onTheGround = null;
        bridgePositions = null;
	}
}

public class RoomDataGenerator : MonoBehaviour
{
    public Tile_ID_Convertion convertor;
    
    /*tilemap的引用*/
    private Tilemap map_ground;
    private Tilemap map_groundDetails;
    private Tilemap map_onTheGround;
    private Tilemap map_bridgeSign;
    private Tilemap map_regionSign;
    /*sign的引用*/
    private TileBase tile_regionSign;
    private TileBase tile_bridgeSign_top;
    private TileBase tile_bridgeSign_bottom;
    private TileBase tile_bridgeSign_left;
    private TileBase tile_bridgeSign_right;

    /*用于设置房间大小的变量（关联到UI)*/
    public int roomWidth = 1, roomHeight = 1;

    /*用于输出序列化数据的可序列化结构*/
    public RoomData data = new RoomData(1,1);
    


    #if UNITY_EDITOR

    /*根据data.sizeW, data.sizeH设置region sign*/
    private void setRegionSign () {
        map_regionSign.ClearAllTiles();
        map_regionSign.SetTile(new Vector3Int(data.sizeW*16-1,data.sizeH*16-1,0),tile_regionSign);
        map_regionSign.FloodFill(Vector3Int.zero, tile_regionSign);
	}


    /*获取必要的引用，包括tilemap和sign*/
    public void setReferences() {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_Ground")
                map_ground = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_GroundDetails")
                map_groundDetails = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_OnTheGround")
                map_onTheGround = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_BridgeSign")
                map_bridgeSign = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_RegionSign")
                map_regionSign = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		}
        tile_regionSign = AssetDatabase.LoadAssetAtPath<TileBase>
        ("Assets/Palettes/roomTiles/region sign.asset");
        tile_bridgeSign_top = AssetDatabase.LoadAssetAtPath<TileBase>
        ("Assets/Palettes/roomTiles/bridge sign top.asset");
        tile_bridgeSign_bottom = AssetDatabase.LoadAssetAtPath<TileBase>
        ("Assets/Palettes/roomTiles/bridge sign bottom.asset");
        tile_bridgeSign_left = AssetDatabase.LoadAssetAtPath<TileBase>
        ("Assets/Palettes/roomTiles/bridge sign left.asset");
        tile_bridgeSign_right = AssetDatabase.LoadAssetAtPath<TileBase>
        ("Assets/Palettes/roomTiles/bridge sign right.asset");
	}


    /*自动重置*/
	private void Reset() {
        setReferences();
        setRegionSign();
	}


    /*调用此函数时将改变地图的大小为roomWidth*roomHeight*/
	public void setSize() {
        //当size的值不符合要求时
        if (roomWidth <= 0 || roomWidth > 3 || roomHeight <= 0 || roomHeight > 3)
            Debug.LogError("roomWidth and roomHeight must be between 1 and 3");

        //更新地图范围
        data.sizeW = roomWidth;
        data.sizeH = roomHeight;
        //引用缺失时重置
        if (map_regionSign == null)
            setReferences();
        setRegionSign();
	}

    
    /*将当前数据保存到Data中准备序列化*/
    public void setData() {
        //引用缺失时重置
        if (map_ground == null)
            setReferences();

        //获取tilemap内容
        BoundsInt region = new BoundsInt(0,0,0,data.sizeW*16,data.sizeH*16,1);
        data.ground = convertor.fromTilesToIDs(map_ground.GetTilesBlock(region));
        data.groundDetails = convertor.fromTilesToIDs(map_groundDetails.GetTilesBlock(region));
        data.onTheGround = convertor.fromTilesToIDs(map_onTheGround.GetTilesBlock(region));

        //获取出入口
        data.bridgePositions = new int[data.sizeW * data.sizeH * 12];
        for (int i = 0; i < data.bridgePositions.Length; i++)
            data.bridgePositions[i] = -1;
        for (int i = 0; i < data.sizeH; i++)
            for (int j = 0; j < data.sizeW; j++) {
                searchBridges(i,j);
			}
	}


    /*调用此函数时将生成房间信息的序列化数据文件*/
    public void generateData(string filename) {
        setData();

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(Application.dataPath+"/Resourse/RoomData/"+filename);

        bf.Serialize(fs,data);

        fs.Close();

        Debug.Log("数据已生成");
        
	}


    /*对第i,j个单元的出入口进行计算*/
    private void searchBridges(int i, int j) {
        int baseX = j * 16, baseY = i * 16; //该单元的基座标
        int baseIndex = (i * data.sizeW + j) * 12; //基索引

        //调查顶部和底部出口,遍历每一行
        for (int k = baseY; k < baseY + 16; k ++) {
            if (map_bridgeSign.GetTile(new Vector3Int(baseX+1,k,0)) == tile_bridgeSign_top) { 
                data.bridgePositions[baseIndex+0] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(baseX+6,k,0)) == tile_bridgeSign_top){
                data.bridgePositions[baseIndex+1] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(baseX+11,k,0)) == tile_bridgeSign_top){ 
                data.bridgePositions[baseIndex+2] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(baseX+4,k,0)) == tile_bridgeSign_bottom){ 
                data.bridgePositions[baseIndex+8] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(baseX+9,k,0)) == tile_bridgeSign_bottom){
                data.bridgePositions[baseIndex+7] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(baseX+14,k,0)) == tile_bridgeSign_bottom){
                data.bridgePositions[baseIndex+6] = k;
                }
		}
        //调查左侧和右侧出口,遍历每一列
        for (int k = baseX; k < baseX + 16; k ++) {
            if (map_bridgeSign.GetTile(new Vector3Int(k,baseY+1,0)) == tile_bridgeSign_left){ 
                data.bridgePositions[baseIndex+9] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(k,baseY+6,0)) == tile_bridgeSign_left){ 
                data.bridgePositions[baseIndex+10] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(k,baseY+11,0)) == tile_bridgeSign_left){ 
                data.bridgePositions[baseIndex+11] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(k,baseY+4,0)) == tile_bridgeSign_right){ 
                data.bridgePositions[baseIndex+5] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(k,baseY+9,0)) == tile_bridgeSign_right){ 
                data.bridgePositions[baseIndex+4] = k;
                }
            if (map_bridgeSign.GetTile(new Vector3Int(k,baseY+14,0)) == tile_bridgeSign_right){ 
                data.bridgePositions[baseIndex+3] = k;
                }
		}

	}

    #endif
	}

