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
	
	[System.Serializable]
	struct actualBounds {
		public float scale;
		public float minX;
		public float minY;
		public float sizeW;
		public float sizeH;
	}
	[SerializeField]
	actualBounds backBounds, middleBounds, frontBounds;

	/*摄像机移动范围和宽高（的一半）*/
    private float boundBottom, boundLeft, boundW, boundH; 
	private float cameraW, cameraH;

	/*实际生成的背景对象的引用*/
	private GameObject background_back;
    private GameObject background_middle;
    private GameObject background_front;


	/*设置背景对象的位置*/
	private void setBgPosition(GameObject ob, actualBounds bounds) {
		/*首先获取摄像机移动的比例*/
		float tx = (gameObject.transform.position.x - boundLeft)/boundW;
		float ty = (gameObject.transform.position.y - boundBottom)/boundH;

		float minx = bounds.minX + cameraW;
		float miny = bounds.minY + cameraH;
		float W = bounds.sizeW - 2*cameraW;
		float H = bounds.sizeH - 2*cameraH;

		ob.transform.position = new Vector3
		(gameObject.transform.position.x - (minx + tx * W),
		gameObject.transform.position.y - (miny + ty * H),
		0);
	}



	private actualBounds setBgBounds(bgOnCamera info)
	{
		actualBounds bounds = new actualBounds();
		if ((info.boundTop - info.boundBottom) / cameraH < (info.boundRight - info.boundLeft) / cameraW )
			bounds.scale = 2*cameraH / (info.boundTop - info.boundBottom) * info.scale;
		else
			bounds.scale = 2*cameraW / (info.boundRight - info.boundLeft) * info.scale;
			
		bounds.minX = info.boundLeft * bounds.scale;
		bounds.minY = info.boundBottom * bounds.scale;
		bounds.sizeW = (info.boundRight - info.boundLeft) * bounds.scale;
		bounds.sizeH = (info.boundTop - info.boundBottom) * bounds.scale;

		return bounds;
	}


	public void init(float left, float right, float top, float bottom)
	{
		boundBottom = bottom;
		boundLeft = left;
		boundW = right - left;
		boundH = top - bottom;

		cameraH = gameObject.GetComponent<Camera>().orthographicSize;
		cameraW = cameraH * gameObject.GetComponent<Camera>().aspect;

		background_back = GameObject.Instantiate<GameObject>(prefab_background_back);
		background_back.transform.parent = gameObject.transform;
		backBounds = setBgBounds(backInfo);
		background_back.transform.localScale = new Vector3
		(backBounds.scale, backBounds.scale, 0);

		background_middle = GameObject.Instantiate<GameObject>(prefab_background_middle);
		background_middle.transform.parent = gameObject.transform;
		middleBounds = setBgBounds(middleInfo);
		background_middle.transform.localScale = new Vector3
		(middleBounds.scale, middleBounds.scale, 0);

		background_front = GameObject.Instantiate<GameObject>(prefab_background_front);
		background_front.transform.parent = gameObject.transform;
		frontBounds = setBgBounds(frontInfo);
		background_front.transform.localScale = new Vector3
		(frontBounds.scale, frontBounds.scale, 0);

		}

	private void Update()
		{
		setBgPosition(background_back,backBounds);
		setBgPosition(background_middle,middleBounds);
		setBgPosition(background_front,frontBounds);
		}
	}
