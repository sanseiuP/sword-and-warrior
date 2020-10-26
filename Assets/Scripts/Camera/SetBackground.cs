using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBackground : MonoBehaviour
{
	/*背景对象的预设*/
    public GameObject prefab_background_back;
    public GameObject prefab_background_middle;
    public GameObject prefab_background_front;
	
	[System.Serializable]
	/*每层背景的摄像机信息*/
    public struct bgOnCamera {
        public float scale;//在摄像机显示的倍数
        public float boundTop;
        public float boundBottom;
        public float boundLeft;
        public float boundRight;
	}
    public bgOnCamera backInfo, middleInfo, frontInfo;

	/*摄像机移动范围和宽高（的一半）*/
    private float boundBottom, boundLeft, boundW, boundH; 
	private float cameraW, cameraH;

	/*实际生成的背景对象的引用*/
	private GameObject background_back;
    private GameObject background_middle;
    private GameObject background_front;


	/*设置背景对象的位置*/
	private void setBgPosition(GameObject ob, bgOnCamera info) {
		/*首先获取摄像机移动的比例*/
		float tx = (gameObject.transform.position.x - boundLeft)/boundW;
		float ty = (gameObject.transform.position.y - boundBottom)/boundH;

		float minx = info.boundLeft*info.scale + cameraW;
		float miny = info.boundBottom*info.scale + cameraH;
		float W = (info.boundRight - info.boundLeft)*info.scale - 2*cameraW;
		float H = (info.boundTop - info.boundBottom)*info.scale - 2*cameraH;

		ob.transform.position = new Vector3
		(gameObject.transform.position.x - (minx + tx * W),
		gameObject.transform.position.y - (miny + ty * H),
		0);
	}

	private void setBgOnCameraInfo() {

	}

	private void setCameraBounds() {
		boundBottom = 0;
		boundLeft = 0;
		boundW = 16;
		boundH = 32;
	}

	private void Start()
		{
		setBgOnCameraInfo();
		setCameraBounds();
		cameraH = gameObject.GetComponent<Camera>().orthographicSize;
		cameraW = cameraH * gameObject.GetComponent<Camera>().aspect;

		background_back = GameObject.Instantiate<GameObject>(prefab_background_back);
		background_back.transform.parent = gameObject.transform;
		background_back.transform.localScale = new Vector3
		(backInfo.scale, backInfo.scale, 0);

		background_middle = GameObject.Instantiate<GameObject>(prefab_background_middle);
		background_middle.transform.parent = gameObject.transform;
		background_middle.transform.localScale = new Vector3
		(middleInfo.scale, middleInfo.scale, 0);

		background_front = GameObject.Instantiate<GameObject>(prefab_background_front);
		background_front.transform.parent = gameObject.transform;
		background_front.transform.localScale = new Vector3
		(frontInfo.scale, frontInfo.scale, 0);

		}

	private void Update()
		{
		setBgPosition(background_back,backInfo);
		setBgPosition(background_middle,middleInfo);
		setBgPosition(background_front,frontInfo);
		}
	}
