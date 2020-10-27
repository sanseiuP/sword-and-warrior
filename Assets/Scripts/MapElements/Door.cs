using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
	public Animator anim; //动画组件
	public int x,y; //在tileMap上的坐标
	private bool open = true;

	/*对地图的引用*/
	public Tilemap map_invisibleBlocks;

	/*对tile的引用*/
	public TileBase tile_invisibleBlock;


	public void init()
	{
		anim = gameObject.GetComponent<Animator>();
		setClose();
	}

	public void setOpen() {
		if (open) {
			Debug.Log("重复的开/关");
			return;
		}
		open = true;

		transform.position = new Vector3
		(map_invisibleBlocks.transform.position.x + x + 0.5f,
		map_invisibleBlocks.transform.position.y + y + 0.5f,
		0);
		anim.SetBool("openDoor", true);

		gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
		
		map_invisibleBlocks.SetTile(new Vector3Int(x,y,0),null);
	}

	public void setClose() {
		if (!open) {
			Debug.Log("重复的开/关");
			return;
		}
		open = false;

		transform.position = new Vector3
		(map_invisibleBlocks.transform.position.x + x + 0.5f,
		map_invisibleBlocks.transform.position.y + y + 0.5f,
		0);
		anim.SetBool("closeDoor", true);

		gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

		map_invisibleBlocks.SetTile(new Vector3Int(x,y,0),tile_invisibleBlock);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(Door))]
public class CustomDoor : Editor
{
    Door m_target;    

	public override void OnInspectorGUI() {
        m_target = target as Door;

		base.OnInspectorGUI();

        if (GUILayout.Button("开门")) {
            m_target.setOpen();
		}

		if (GUILayout.Button("关门")) {
            m_target.setClose();
		}
	}
}
#endif
