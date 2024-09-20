using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardScript : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    private void Awake()
    {
        tileMap.CompressBounds();
    }

    public TileBase[] GetAllTiles()
    {
        return tileMap.GetTilesBlock(tileMap.cellBounds);
    }

    public TileBase GetTileByID(int tileID)
    {
        return GetAllTiles()[tileID];
    }

    private int GetTileID(TileBase tile)
    {
        var allTiles = GetAllTiles();
        for (int i = 0; i < allTiles.Length; i++)
        {
            if (allTiles[i] == tile) return 0;
        }
        return 0;
    }
}
