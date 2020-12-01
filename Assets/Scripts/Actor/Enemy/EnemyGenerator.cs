using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator Instance { get; private set; }

    public GameObject WaterSlime;
    public GameObject target;
    public GameObject bleed;

    private Hashtable enemyIndex = new Hashtable();

    private void buildIndex()
    {
        enemyIndex.Add("WaterSlime", WaterSlime);
    }
    public void enemyGen(string name, float xpos, float ypos)
    {
        Vector2 genPos = new Vector2(xpos, ypos);
        Object genEnemy = (Object)enemyIndex[name];
        GameObject enemy=(GameObject)Instantiate(genEnemy, genPos, Quaternion.identity);

      
    }

     void Awake()
    {
        Instance = this;
        buildIndex();
        enemyGen("WaterSlime", 0, 0);
    }
}