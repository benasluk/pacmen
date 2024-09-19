using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SignalRConnector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject connectButton;
    [SerializeField] private GameObject cover;
    

    private HubConnection connection;

    public async void ConnectToServer()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7255/Server")
            .Build();

        connection.On<SharedLibs.Positions>("ReceiveMap", ReceiveMap);

        await connection.StartAsync();
        Debug.Log("Connection started");
        connectButton.SetActive(false);
        cover.SetActive(false);
    }

    public async void SendSomeMessage()
    {
        string message = "This message is from connection " + connection.ConnectionId.ToString();
        try
        {
            await connection.InvokeAsync("ReceivedMessage", message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error sending message: " + ex.Message);
        }
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
}
