using Microsoft.AspNetCore.SignalR;
using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;
using System.Diagnostics;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly MessageService _messageService;
        private readonly GameService _gameService;
        private readonly GameLoop _gameLoop;

        private string pausedById;
        public GameHub (MessageService messageService, GameService gameService, GameLoop gameLoop)
        {
            _gameService = gameService;
            _messageService = messageService;
            _gameLoop = gameLoop;
        }
        public async Task ReceivedDirection(SharedLibs.PacmanMovement movement)
        {
            await Console.Out.WriteLineAsync(movement.PlayerId);
            await Console.Out.WriteLineAsync(movement.Direction.ToString());
            _messageService.StorePlayerInput(Context.ConnectionId, movement);
        }
        public override Task OnConnectedAsync()
        {
            var playerid = Context.ConnectionId;
            Console.WriteLine("Got a connection from Player ID " + playerid + " !");

            return base.OnConnectedAsync();
        }
        public async Task LevelChange(int num)
        {
            Console.WriteLine("Changing level");
            _messageService.StoreLevelChange(num);
        }
        
    }
}
