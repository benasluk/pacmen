using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardScript : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;

    private SharedLibs.Positions serverMap;
    private void Awake()
    {
        tileMap.CompressBounds();
    }

    public void UpdateMap(SharedLibs.Positions newMap)
    {
        serverMap = newMap;
    }
}
