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
    

    private HubConnection connection;

    private void Awake()
    {
        //coverCanvas.SetActive(false);
    }

    public async void ConnectToServer()
    {
        var handshake = new SharedLibs.HandShake();

        handshake.PlayerName = usernameField.text;

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
    }

    private void DisableObject(GameObject toDisable)
    {
        toDisable.SetActive(false);
    }

    private void HandshakeReceived(string welcomeMessage)
    {
        //Debug.Log("Connection started");
        Debug.Log(welcomeMessage);
        DisableObject(coverCanvas);
        //Debug.Log("Test");
    }
}
