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

    //Kampu reikia pacmano pozicijos nustatymui
    [SerializeField] private GameObject topLeft;
    [SerializeField] private GameObject bottomRight;

    [Header("Attributes")]
    [SerializeField] private float speed = 0.5f;


    private void Awake()
    {
        //Snappinam pacmana tiesiai i grid block
        pacman.transform.position = tileMap.CellToWorld(tileMap.WorldToCell(transform.position)) + tileMap.layoutGrid.transform.lossyScale / 2;

        //Snappinam kampus tiesiai i grid blockus
        topLeft.transform.position = tileMap.CellToWorld(tileMap.WorldToCell(topLeft.transform.position)) + tileMap.layoutGrid.transform.lossyScale / 2;
        bottomRight.transform.position = tileMap.CellToWorld(tileMap.WorldToCell(bottomRight.transform.position)) + tileMap.layoutGrid.transform.lossyScale / 2;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log(tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }

    private void HandleMovement(string direction)
    {
        switch (direction)
        {
            case "Right":
                if(!tileMap.GetTile(tileMap.WorldToCell(transform.position) + Vector3Int.right).name.Contains("Wall"))
                {
                    transform.position = tileMap.CellToWorld(tileMap.WorldToCell(transform.position) + Vector3Int.right) + tileMap.layoutGrid.transform.lossyScale / 2;
                }
                break;
            default: 
                break;
        }
    }

    private Vector3 GetCurrentBlock()
    {
        return new Vector3();
    }
}
