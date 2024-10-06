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
    [SerializeField] private GameObject tileMap;
    [SerializeField] private TextMeshProUGUI connectedPlayers;
    [SerializeField] private GameObject waitingForPlayersText;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameObject clientPacman;


    private HubConnection connection;

    private void OnApplicationQuit()
    {
        connection.StopAsync();
    }
    public async void ConnectToServer(bool isDefault)
    {
        var handshake = new SharedLibs.HandShake(usernameField.text.Trim((char)8203));

        string serverIP;

        if (isDefault) serverIP = "http://127.0.0.1:5076/Server";
        else serverIP = serverField.text.Trim((char)8203);

        Debug.Log(serverIP);

        connection = new HubConnectionBuilder()
        .WithUrl(serverIP).AddNewtonsoftJsonProtocol(options =>
        {
            options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }).Build();

        connection.On<Positions>("ReceiveMap", ReceiveMap);
        connection.On<string>("HandshakeReceived", HandshakeReceived);
        connection.On<TileStatus>("ReceivePacman", SetPacman);
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
            if (!tileMap.activeInHierarchy)
            {
                tileMap.SetActive(true);
                clientPacman.SetActive(true);
            }
            tileMap.GetComponent<BoardScript>().UpdateMap(map);
            clientPacman.GetComponent<PacmanScript>().SnapToMapLocation();
        });    
    }

    public void UpdatePlayerCount(int newCount)
    {
        string newText = connectedPlayers.text.ToString().Substring(0, connectedPlayers.text.Length - 1) + newCount.ToString();
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (newCount >= 1)
            {
                Debug.Log("Setting pacman canMove to True");
                waitingForPlayersText.SetActive(false);
                if (!clientPacman.GetComponent<SpriteRenderer>().enabled) clientPacman.GetComponent<SpriteRenderer>().enabled = true;
                clientPacman.GetComponent<PacmanScript>().SetCanMove(true);
            }
            else
            {
                waitingForPlayersText.SetActive(true);
                clientPacman.GetComponent<PacmanScript>().SetCanMove(false);
            }
            connectedPlayers.text = newText;
        });
    }

    public void UpdateTimer(int newTime)
    {
        newTime /= 1000;
        string newText = timer.text.ToString().Substring(0, timer.text.Length - 5) + (newTime/60).ToString("00") + ':' + (newTime%60).ToString("00");
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            timer.text = newText;
        });
    }

    public void SetPacman(TileStatus pacman)
    {
        Debug.Log("Received pacman");
        switch (pacman)
        {
            case TileStatus.Pacman1:
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    clientPacman.GetComponent<PacmanScript>().SetPacmanNumber(1);
                });
                break;
            case TileStatus.Pacman2:
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    clientPacman.GetComponent<PacmanScript>().SetPacmanNumber(2);
                });
                break;
            case TileStatus.Pacman3:
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    clientPacman.GetComponent<PacmanScript>().SetPacmanNumber(3);
                });
                break;
            case TileStatus.Pacman4:
                MainThreadDispatcher.Instance().Enqueue(() =>
                {
                    clientPacman.GetComponent<PacmanScript>().SetPacmanNumber(4);
                });
                break;
        }
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
