using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LeaderboardScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    private Dictionary<int, Vector3> panelPositions = new Dictionary<int, Vector3>();
    private Dictionary<string, int> standings = new Dictionary<string, int>();
    void Start()
    {
        for(int i = 1; i <= panels.Count; i++)
        {
            panelPositions.Add(i, panels[i].transform.position);
            standings.Add(panels[i].GetComponentInChildren<Image>().sprite.ToString().Split('_')[0], i);
        }
    }

    private void UpdatePosition(GameObject panel, int newPlace)
    {
        int oldPlace = standings[panel.GetComponentInChildren<Image>().sprite.ToString().Split('_')[0]];

        // Get the panel currently at the new position
        GameObject panelAtNewPlace = panels[newPlace];

        // Swap positions
        Vector3 oldPosition = panel.transform.position;
        panel.transform.position = panelPositions[newPlace];
        panelAtNewPlace.transform.position = oldPosition;

        // Update the standings
        standings[panel.GetComponentInChildren<Image>().sprite.ToString().Split('_')[0]] = newPlace;
        standings[panelAtNewPlace.GetComponentInChildren<Image>().sprite.ToString().Split('_')[0]] = oldPlace;

        // Update the panel list to reflect the new order
        panels[oldPlace] = panelAtNewPlace;
        panels[newPlace] = panel;
    }
    private void UpdateScore(GameObject panel, int newScore)
    {

    }

}
