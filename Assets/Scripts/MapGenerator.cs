using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    public RoomData[] allRoomsData;
    public Tilemap map_ground;
    public Tilemap map_groundDetails;
    public Tilemap map_onTheGround;

    public Tile_ID_Convertion Convertor;

    public void setReferences() {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_Ground")
                map_ground = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_GroundDetails")
                map_groundDetails = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		    if (gameObject.transform.GetChild(i).tag == "Tilemap_Room_OnTheGround")
                map_onTheGround = gameObject.transform.GetChild(i).GetComponent<Tilemap>();
		}
        Convertor = AssetDatabase.LoadAssetAtPath<Tile_ID_Convertion>
        ("Assets/Resourse/Tile_ID_Convertor.asset");
	}

    public void getRoomsData() {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Open(Application.dataPath+"/Resourse/RoomData/AllRoomsData", FileMode.Open);

        allRoomsData = bf.Deserialize(fs) as RoomData[];

        fs.Close();
	}

    public void generateSampleRoom()
    {
        if (allRoomsData == null)   getRoomsData();
        if (allRoomsData.Length == 0)   {
            Debug.Log("No Room Data Found");
            return;
		}
        if (map_ground == null) setReferences();

        int index = UnityEngine.Random.Range(0, allRoomsData.Length);
        paintRoom(0,0,allRoomsData[index]);
	}

    private void paintRoom(int x, int y, RoomData data) {
        BoundsInt region = new BoundsInt(x,y,0,data.sizeW*16, data.sizeH*16,1);
        map_ground.ClearAllTiles();
        map_groundDetails.ClearAllTiles();
        map_onTheGround.ClearAllTiles();
        map_ground.SetTilesBlock(region,Convertor.fromIDsToTiles(data.ground));
        map_groundDetails.SetTilesBlock(region,Convertor.fromIDsToTiles(data.groundDetails));
        map_onTheGround.SetTilesBlock(region,Convertor.fromIDsToTiles(data.onTheGround));
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
	}
}