using SharedLibs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacmanScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private List<Sprite> pacmen;

    [Header("Attributes")]
    [SerializeField] private float speed = 0.5f;
    private SignalRConnector signalRConnector;
    private GameObject spawnPoint;
    private string pacmanColor;

    private void Start()
    {
        signalRConnector = FindObjectOfType<SignalRConnector>();
    }

    private void Update()
    {
        Direction dir = Direction.None;
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            dir = Direction.Up;
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            dir = Direction.Down;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            dir = Direction.Left;
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            dir = Direction.Right;
        }
        if (dir != Direction.None)
        {
            signalRConnector.SendDirection(dir);
        }
    }

    public void SetPacmanNumber(int num)
    {
        Debug.Log($"Setting pacman to number{num}");
        switch (num)
        {
            case 1:
                GetComponent<SpriteRenderer>().sprite = pacmen.First(p => p.name.Contains("Green"));
                spawnPoint = GameObject.Find("GreenSpawn");
                pacmanColor = "Green";
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = pacmen.First(p => p.name.Contains("Red"));
                spawnPoint = GameObject.Find("RedSpawn");
                pacmanColor = "Red";
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = pacmen.First(p => p.name.Contains("Yellow"));
                spawnPoint = GameObject.Find("YellowSpawn");
                pacmanColor = "Yellow";
                break;
            case 4:
                GetComponent<SpriteRenderer>().sprite = pacmen.First(p => p.name.Contains("Purple"));
                spawnPoint = GameObject.Find("PurpleSpawn");
                pacmanColor = "Purple";
                break;
        }

        transform.position = tileMap.CellToWorld(tileMap.WorldToCell(spawnPoint.transform.position)) + tileMap.layoutGrid.transform.lossyScale / 2;
    }

    public void SnapToMapLocation()
    {
        foreach (var pos in tileMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tileMap.GetTile(pos);
            if (tile.name.Contains(pacmanColor))
            {
                transform.position = tileMap.CellToWorld(pos) + tileMap.layoutGrid.transform.lossyScale / 2;
            }
        }
    }
}
