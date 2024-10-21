using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class LeaderboardScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    private List<Vector3> panelPositions = new List<Vector3>();

    private List<string> order;

    void Start()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panelPositions.Add(panels[i].transform.position);
        }
        order = panels.Select(p => p.name).ToList();
    }

    public void UpdateScoreboard(int[] newScores)
    {
        for (int i = 0; i < newScores.Length; i++)
        {
            UpdateScore(panels[i], newScores[i]);
        }

        Debug.Log(panels[0].GetComponentInChildren<TextMeshProUGUI>().text.Split(' ')[1]);
        order = panels.OrderByDescending(p => p.GetComponentInChildren<TextMeshProUGUI>().text.Split(' ')[1]).Select(p => p.name).ToList();

        UpdatePositions();
    }

    private void UpdatePositions()
    {
        for(int i = 0; i < panels.Count; i++) 
        {
            Vector3 newPos = panelPositions[order.IndexOf(panels[i].name)];
            GameObject panelToUpdate = panels[i];
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                panelToUpdate.transform.position = newPos;
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
