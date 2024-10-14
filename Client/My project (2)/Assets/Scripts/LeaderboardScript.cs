using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LeaderboardScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    private Dictionary<int, Vector3> panelPositions = new Dictionary<int, Vector3>();

    void Start()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panelPositions.Add(i, panels[i].transform.position);
        }
    }

    public void UpdateScoreboard(int[] newScores)
    {
        for (int i = 0; i < newScores.Length; i++)
        {
            UpdateScore(panels[i], newScores[i]);
        }

        //for (int i = 0; i < panels.Count; i++)
        //{
            //if (int.Parse(panels[i].GetComponentInChildren<TextMeshProUGUI>().text.Split(' ')[1]) >= 10) UpdatePosition(panels[i], 2);
        //}

    }

    private void UpdatePosition(GameObject panel, int newPlace)
    {
        if (panel.transform.position != panelPositions[newPlace])
        {
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                 panel.transform.position = panelPositions[newPlace];
            });
        }
    }

    private void UpdateScore(GameObject panel, int newScore)
    {
        if (int.Parse(panel.GetComponentInChildren<TextMeshProUGUI>().text.Split(' ')[1]) != newScore)
        {
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                panel.GetComponentInChildren<TextMeshProUGUI>().text = "score: " + newScore.ToString();
            });
        }
    }
}
