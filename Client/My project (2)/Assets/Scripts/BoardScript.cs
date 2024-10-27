using SharedLibs;
using System;
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

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log(tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - topLeftCellPosition);
        }
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
                    case TileStatus.Pacman1:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Green_pac")));
                        break;
                    case TileStatus.Pacman2:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Red_pac")));
                        break;
                    case TileStatus.Pacman3:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Yellow_pac")));
                        break;
                    case TileStatus.Pacman4:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Purple_pac")));
                        break;
                    case TileStatus.Ghost1:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Ghost_R")));
                        break;
                    case TileStatus.Ghost2:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Ghost_P")));
                        break;
                    case TileStatus.Ghost3:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Ghost_C")));
                        break;
                    case TileStatus.Ghost4:
                        tileMap.SetTile(topLeftCellPosition + new Vector3Int(j, -i, 0), tiles.First(t => t.name.Contains("Ghost_O")));
                        break;
                }
            }
        }
    }
}
