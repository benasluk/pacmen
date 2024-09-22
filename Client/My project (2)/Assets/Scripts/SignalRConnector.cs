using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SharedLibs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
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
    [SerializeField] private TextMeshProUGUI connectedPlayers;
    [SerializeField] private TextMeshProUGUI timer;


    private HubConnection connection;

    private void OnApplicationQuit()
    {
        connection.StopAsync();
    }
    public async void ConnectToServer()
    {
        var handshake = new SharedLibs.HandShake(usernameField.text.Trim((char)8203));

        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7255/Server").AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).Build();

        connection.On<Positions>("ReceiveMap", ReceiveMap);
        connection.On<string>("HandshakeReceived", HandshakeReceived);
        connection.On<string>("HandshakeFailed", HandshakeFailed);
        connection.On<int>("UpdatePlayerCount", UpdatePlayerCount);
        connection.On<int>("UpdateTimer", UpdateTimer);

        await connection.StartAsync();
        await connection.SendAsync("Handshake", handshake);
    }
    public async void SendDirection(Direction dir)
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            Debug.Log("send movement" + dir.ToString());
            PacmanMovement pacmanMovement = new PacmanMovement();
            pacmanMovement.Direction = dir;
            pacmanMovement.PlayerId = connection.ConnectionId;
            await connection.SendAsync("ReceivedDirection", pacmanMovement);
        }
    }

    public void ReceiveMap(SharedLibs.Positions map)
    {
        Debug.Log("Map received.");
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            tileMap.GetComponent<BoardScript>().UpdateMap(map);
        });    
    }

    public void UpdatePlayerCount(int newCount)
    {
        string newText = connectedPlayers.text.ToString().Substring(0, connectedPlayers.text.Length - 1) + newCount.ToString();
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            connectedPlayers.text = newText;
        });
    }

    public void UpdateTimer(int newTime)
    {
        newTime /= 1000;
        string newText = timer.text.ToString().Substring(0, timer.text.Length - 5) + (newTime/60).ToString("00") + ':' + (newTime%60).ToString("00");
        Debug.Log(newText);
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            timer.text = newText;
        });
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
