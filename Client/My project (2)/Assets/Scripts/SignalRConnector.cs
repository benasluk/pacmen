using Microsoft.AspNetCore.SignalR.Client;
using SharedLibs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SignalRConnector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject coverCanvas;
    [SerializeField] private TextMeshProUGUI usernameField;
    [SerializeField] private GameObject invalidText;
    [SerializeField] private GameObject tileMap;
    

    private HubConnection connection;

    private void OnApplicationQuit()
    {
        connection.StopAsync();
    }

    public async void ConnectToServer()
    {
        var handshake = new SharedLibs.HandShake
        {
            PlayerName = usernameField.text.Trim((char)8203) //Trim reikia, nes unity dadeda sita char gale, o ten empty char
        };

        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7255/Server")
            .Build();

        connection.On<Positions?>("ReceiveMap", ReceiveMap);
        connection.On<string>("HandshakeReceived", HandshakeReceived);
        connection.On<string>("HandshakeFailed", HandshakeFailed);
        connection.On<string>("Test", (test) => Debug.Log(test));

        await connection.StartAsync();
        await connection.SendAsync("Handshake", handshake);
    }

    public void ReceiveMap(SharedLibs.Positions map)
    {
        Debug.Log("Map received.");
        //Debug.Log("First value of map: " + map.Grid[0, 0].ToString());
    }

    private void UpdateMap(SharedLibs.Positions newMap)
    {

    }

    public async void HandshakeFailed(string error)
    {
        Debug.Log("Failed to connect to server!");
        Debug.Log("Error: " + error);
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            invalidText.SetActive(true);
        });
        await connection.StopAsync();
    }

    private void HandshakeReceived(string welcomeMessage)
    {
        Debug.Log(welcomeMessage);
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            coverCanvas.SetActive(false);
        });
    }
}
