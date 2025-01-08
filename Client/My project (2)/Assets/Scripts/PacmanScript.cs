using SharedLibs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacmanScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private List<Sprite> pacmen;
    [SerializeField] private GameObject pacmanColorText;

    [Header("Attributes")]
    [SerializeField] private float speed = 0.5f;
    private SignalRConnector signalRConnector;
    private GameObject spawnPoint;
    public string pacmanColor;
    private Quaternion rotation;
    private bool canMove;
    private bool isPlayerDead;

    private void Start()
    {
        signalRConnector = FindObjectOfType<SignalRConnector>();
        rotation = transform.rotation;
        canMove = false;
        isPlayerDead = false;
    }

    public void SetPlayerDead(bool isDead)
    {
        isPlayerDead = isDead;
    }

    private void Update()
    {
        if(canMove)
        {
            Direction dir = Direction.None;
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                dir = Direction.Up;
                rotation.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                dir = Direction.Down;
                rotation.eulerAngles = new Vector3(0, 0, 270);
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                dir = Direction.Left;
                rotation.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                dir = Direction.Right;
                rotation.eulerAngles = new Vector3(0, 0, 0);
            }
            if (dir != Direction.None)
            {
                signalRConnector.SendDirection(dir);
            }
        }
    }

    public void RotatePacman()
    {
        transform.rotation = rotation;
        rotation = transform.rotation;
    }

    public void SetCanMove(bool can)
    {
        canMove = can;
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
        pacmanColorText.GetComponent<TextMeshProUGUI>().text += pacmanColor;
        pacmanColorText.GetComponent<TextMeshProUGUI>().color = (Color)typeof(Color).GetProperty(pacmanColor.ToLowerInvariant()).GetValue(null, null); //Stack overflow magic
    }

    public void SnapToMapLocation(bool isSceneChange)
    {
        if (isSceneChange)
        {
            isPlayerDead = false;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        if(!isPlayerDead)
        {
            foreach (var pos in tileMap.cellBounds.allPositionsWithin)
            {
                TileBase tile = tileMap.GetTile(pos);
                if (tile.name.Contains(pacmanColor + "_pacman"))
                {
                    transform.position = tileMap.CellToWorld(pos) + tileMap.layoutGrid.transform.lossyScale / 2;
                    RotatePacman();
                    return;
                }
            }
            isPlayerDead = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
