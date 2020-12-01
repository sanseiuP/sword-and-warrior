using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public void enemyGen(string name, float xpos, float ypos)
    {
        Vector2 genPos = new Vector2(xpos, ypos);
        Object genEnemy = GameObject.Find(name);
        Object gen = Instantiate(genEnemy, genPos, Quaternion.identity);
    }

    private void Start()
    {
        enemyGen("WaterSlime", 0, 0);
    }
}
