using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomDataGenerator))]
public class CustomRoomData : Editor
{
	RoomDataGenerator m_target;	

	public override void OnInspectorGUI() {
		m_target = target as RoomDataGenerator;	

		base.OnInspectorGUI();

		if (GUILayout.Button("重置引用")) {
			m_target.setReferences();
		}

		if (GUILayout.Button("应用大小")) {
			m_target.setSize();
		}

		if (GUILayout.Button("生成数据")) {
			m_target.generateData("tempRoomData.txt");
		}
	}
}
