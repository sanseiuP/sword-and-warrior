using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile_ID_Convertor", menuName = "Tile_ID_Convertor", order = 1)]
public class Tile_ID_Convertion : ScriptableObject
{
    public TileBase[] tileBases = new TileBase[1];

    public int fromTileToID(TileBase tile) {
        if (tile == null)   return -1;

        for (int i = 0; i < tileBases.Length; i ++)
            if (tileBases[i] == tile)
                return i;

        return -1;
	}

    public TileBase fromIDToTile(int i) {
        if (i >= 0 && i < tileBases.Length)
            return tileBases[i];

        return null;
	}

    public int[] fromTilesToIDs(TileBase[] tiles) {
        int[] ids = new int[tiles.Length];
        for (int i = 0; i < tiles.Length; i ++)
            ids[i] = fromTileToID(tiles[i]);

        return ids;
	}

    public TileBase[] fromIDsToTiles(int[] ids) {
        TileBase[] tiles = new TileBase[ids.Length];
        for (int i = 0; i < ids.Length; i ++)
            tiles[i] = fromIDToTile(ids[i]);

        return tiles;
	}
} 
