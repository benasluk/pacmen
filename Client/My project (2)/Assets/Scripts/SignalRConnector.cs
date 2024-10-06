using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SharedLibs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    [SerializeField] private TextMeshProUGUI serverField;
    [SerializeField] private GameObject invalidText;
    

    private HubConnection connection;

    private void OnApplicationQuit()
    {
        connection.StopAsync();
    }
    public async void ConnectToServer(bool isDefault)
    {
        var handshake = new SharedLibs.HandShake(usernameField.text.Trim((char)8203));

        string serverIP;

        if (isDefault) serverIP = "http://127.0.0.1:5026/Server";
        else serverIP = serverField.text.Trim((char)8203);

        connection = new HubConnectionBuilder()
            .WithUrl(serverIP).AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).Build();

        connection.On<Positions>("ReceiveMap", ReceiveMap);
        connection.On<string>("HandshakeReceived", HandshakeReceived);
        connection.On<string>("HandshakeFailed", HandshakeFailed);
        connection.On<string>("Test", (test) => Debug.Log(test));

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
