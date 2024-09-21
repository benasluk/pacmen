using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SignalRConnector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject coverCanvas;
    [SerializeField] private TextMeshProUGUI usernameField;
    [SerializeField] private GameObject invalidText;
    

    private HubConnection connection;

    private void Awake()
    {
        //coverCanvas.SetActive(false);
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

        connection.On<SharedLibs.Positions>("ReceiveMap", ReceiveMap);
        connection.On<string>("HandshakeReceived", HandshakeReceived);
        connection.On<string>("HandshakeFailed", HandshakeFailed);

        await connection.StartAsync();
        await connection.SendAsync("Handshake", handshake);
    }

    public async void ReceiveMap(SharedLibs.Positions map)
    {
        Debug.Log("Map received.");
        Debug.Log(map.Grid[5,11]);
        Debug.Log("First value of map: " + map.Grid[0, 0].ToString());
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
