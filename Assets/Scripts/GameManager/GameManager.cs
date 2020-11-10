using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    /*需要创建整个场景的大地图，并绑定MapGenerator*/
    public GameObject map;

    /*需要创建人物*/
    public GameObject warrior;

    public GameObject enemy;

    /*摄像机*/
    public GameObject cam;

    public int sizeW = 5, sizeH = 5, area = 10;

    // Start is called before the first frame update
    void Start()
    {
       regenerate();
    }

    void regenerate() 
    {
         map.GetComponent<MapGenerator>().generateMapInArea(sizeW, sizeH, area);
        Vector2Int pos = map.GetComponent<MapGenerator>().getRandomPosition();
        warrior.transform.position = new Vector3(pos.x, pos.y);
        enemy.transform.position = new Vector3(pos.x+3, pos.y+3);
        cam.GetComponent<SetBackground>().init(0, sizeW*16, sizeH*16, 0);
        timer = 2;
    }

    float timer = 2;
    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer--;
            AstarPathEditor.MenuScan();
        }
        if (Input.GetKeyDown(KeyCode.R))
            regenerate();
    }
}
