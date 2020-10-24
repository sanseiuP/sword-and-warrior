using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateRoomTemplate
{
    [MenuItem("GameObject/CustomGenerate/RoomTemplate",false,10)]
    public static void generateRoomTemplate(MenuCommand menuCommand) {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>
        ("Assets/Prefabs/Rooms/RoomTemplate.prefab");
        GameObject go = GameObject.Instantiate<GameObject>(prefab);
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
	}
}
