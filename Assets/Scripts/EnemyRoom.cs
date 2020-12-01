using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyRoom 
{
    public EnemyGenerator enemygenrator;
    public Tilemap ground, ontheground;


    public void GenSomeEnemy(int x, int y, int w, int h,string name,int num)
    {
        while (num > 0)
        {
            TileBase tile1,tile2;
            int nx, ny;
            do
            {
                nx = UnityEngine.Random.Range(x, w + x);
                ny = UnityEngine.Random.Range(y, h + y);
                Vector3Int pos = new Vector3Int(nx, ny, 0);
                tile1 = ground.GetTile(pos);
                tile2 = ontheground.GetTile(pos);
            } while (tile1 == null || tile2 != null);
            

            enemygenrator.enemyGen(name,nx, ny);
            num--;
        }

    }
}
