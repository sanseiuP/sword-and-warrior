using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class RoomDataManager : MonoBehaviour
{
    [SerializeField]
    public RoomData[] data;
    public GameObject[] rooms = new GameObject[0];

    public void generateData() {
        data = new RoomData[rooms.Length];
        for(int i = 0; i < rooms.Length; i++) {
            rooms[i].GetComponent<RoomDataGenerator>().setData();
            data[i] = rooms[i].GetComponent<RoomDataGenerator>().data;

        BinaryFormatter bf = new BinaryFormatter();

        FileStream fs = File.Create(Application.dataPath+"/Resourse/RoomData/AllRoomsData");

        bf.Serialize(fs,data);

        fs.Close();

        Debug.Log("数据已生成");
		}
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(RoomDataManager))]
public class CustomRoomDataManager : Editor
{
    RoomDataManager m_target;    

	public override void OnInspectorGUI() {
        m_target = target as RoomDataManager;

		base.OnInspectorGUI();

        if (GUILayout.Button("生成数据")) {
            m_target.generateData();
		}
	}
}
#endif