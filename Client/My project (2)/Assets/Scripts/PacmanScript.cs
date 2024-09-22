using SharedLibs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacmanScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pacman;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private GameObject spawnPoint;

    [Header("Attributes")]
    [SerializeField] private float speed = 0.5f;
    private SignalRConnector signalRConnector;

    private void Start()
    {
        signalRConnector = FindObjectOfType<SignalRConnector>();
    }
    private void Awake()
    {
        //Snappinam pacmana tiesiai i grid block
        pacman.transform.position = tileMap.CellToWorld(tileMap.WorldToCell(transform.position)) + tileMap.layoutGrid.transform.lossyScale / 2;
    }

    private void Update()
    {
        Direction dir = Direction.None;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir = Direction.Down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir = Direction.Right;
        }
        if (dir != Direction.None)
        {
            signalRConnector.SendDirection(dir);
        }
    }

    private void HandleMovement(string direction)
    {
        switch (direction)
        {
            case "Right":
                if (!tileMap.GetTile(tileMap.WorldToCell(transform.position) + Vector3Int.right).name.Contains("Wall"))
                {
                    transform.position = tileMap.CellToWorld(tileMap.WorldToCell(transform.position) + Vector3Int.right) + tileMap.layoutGrid.transform.lossyScale / 2;
                }
                break;
            default:
                break;
        }
    }
}
