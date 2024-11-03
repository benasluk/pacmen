using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
    [SerializeField] private LeaderboardScript leaderboard;
    [SerializeField] private GameObject changeLevelButton;
    [SerializeField] private GameObject pausedOverlayCanvas;
    [SerializeField] private MapDecorator decoratorScript;

    private int currLevel;
    private HubConnection connection;
    private bool pausedByThis;

    private void OnApplicationQuit()
    {
        connection.StopAsync();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            PauseUnpause(true);
        }
    }
    public async void ConnectToServer(bool isDefault)
    {
        var handshake = new SharedLibs.HandShake(usernameField.text.Trim((char)8203));

        string serverIP;

        currLevel = 0;

        if (isDefault) serverIP = "http://127.0.0.1:5076/Server";
        else serverIP = serverField.text.Trim((char)8203);

        connection = new HubConnectionBuilder()
        .WithUrl(serverIP).AddNewtonsoftJsonProtocol(options =>
        {
            options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;

            options.PayloadSerializerSettings.Error = (sender, args) => 
            {
                Debug.Log($"JSON Serialization Error: {args.ErrorContext.Error.Message}");
                args.ErrorContext.Handled = true;
            };
        }).Build();

        connection.On<Positions>("ReceiveMap", ReceiveMap);
        connection.On<string>("HandshakeReceived", HandshakeReceived);
        connection.On<TileStatus>("ReceivePacman", SetPacman);
        connection.On<string>("HandshakeFailed", HandshakeFailed);
        connection.On<int>("UpdatePlayerCount", UpdatePlayerCount);
        connection.On<int>("UpdateTimer", UpdateTimer);
        connection.On<bool, string>("SetPaused", SetPaused);

        await connection.StartAsync();
        await connection.SendAsync("Handshake", handshake);
    }
    public async void SendDirection(Direction dir)
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            PacmanMovement pacmanMovement = new PacmanMovement();
            pacmanMovement.Direction = dir;
            pacmanMovement.PlayerId = connection.ConnectionId;
            await connection.SendAsync("ReceivedDirection", pacmanMovement);
        }
    }

    public void ReceiveMap(SharedLibs.Positions map)
    {
        Debug.Log("Got a new map");
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (!tileMap.activeInHierarchy)
            {
                tileMap.SetActive(true);
                clientPacman.SetActive(true);
            }
            tileMap.GetComponent<BoardScript>().UpdateMap(map);
            clientPacman.GetComponent<PacmanScript>().SnapToMapLocation();
            leaderboard.UpdateScoreboard(map.Scores);
            decoratorScript.SetAddons(map.Addons);
        });
    }

    public void UpdatePlayerCount(int newCount)
    {
        string newText = connectedPlayers.text.ToString().Substring(0, connectedPlayers.text.Length - 1) + newCount.ToString();
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (newCount >= 1)
            {
                waitingForPlayersText.SetActive(false);
                changeLevelButton.SetActive(true);
                if (!clientPacman.GetComponent<SpriteRenderer>().enabled) clientPacman.GetComponent<SpriteRenderer>().enabled = true;
                clientPacman.GetComponent<PacmanScript>().SetCanMove(true);
            }
            else
            {
                waitingForPlayersText.SetActive(true);
                changeLevelButton.SetActive(false);
                clientPacman.GetComponent<PacmanScript>().SetCanMove(false);
            }
            connectedPlayers.text = newText;
        });
    }

    public void UpdateTimer(int newTime)
    {
        newTime /= 1000;
        string newText = timer.text.ToString().Substring(0, timer.text.Length - 5) + (newTime / 60).ToString("00") + ':' + (newTime % 60).ToString("00");
        MainThreadDispatcher.Instance().Enqueue(() =>
        {
            timer.text = newText;
        });
    }

    public void SetPacman(TileStatus pacman)
    {
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

    public async void SetAddonInServer(Addon toSet)
    {
        Debug.Log("Setting map addon " + toSet.GetType().ToString() + " in server");
        await connection.SendAsync("UpdateGameMapAddons", toSet.GetType(), toSet.GetValue());
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

    public void ChangeLevel()
    {
        connection.SendAsync("LevelChange", ++currLevel % 2);
        Debug.Log("Sending change level signal for level " + (currLevel % 2) + 1);
    }

    public void PauseUnpause(bool pause)
    {
        Debug.Log("Triggering pause/unpause action");
        if(pause)
        {
            connection.SendAsync("ReceiveCommand", CommandType.Pause, CommandAction.Execute);
        }
        else if(!pause) 
        {
            connection.SendAsync("ReceiveCommand", CommandType.Pause, CommandAction.Undo);
        }
    }

    private void SetPaused(bool paused, string pausedById)
    {
        if (paused)
        {
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                pausedOverlayCanvas.SetActive(true);
                if(pausedById == connection.ConnectionId)
                {
                    pausedOverlayCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                }
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() =>
            {
                pausedOverlayCanvas.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                pausedOverlayCanvas.SetActive(false);
            });
        }
    }
}
