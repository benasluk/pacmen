using SharedLibs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI.Table;

public class BoardScript : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private GameObject topLeft;
    [SerializeField] private List<TileBase> tiles;

    private Vector3Int topLeftCellPosition;

    private void Awake()
    {
        tileMap.CompressBounds();
        topLeftCellPosition = tileMap.WorldToCell(topLeft.transform.position);
    }

    public void UpdateMap(SharedLibs.Positions newMap)
    {
        for (int i = 0; i < 36; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                switch (newMap.Grid[i, j])
                {
                    case TileStatus.Empty:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Nothing")));
                        break;
                    case TileStatus.Pellet:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Pellet")));
                        break;
                    case TileStatus.Wall:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Wall")));
                        break;
                }
            }
        }
    }
}
