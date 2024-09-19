using Microsoft.AspNetCore.SignalR;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;
namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly MessageService _messageService;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GameLoop _gameLoop;
        public GameHub (MessageService messageService, GameService gameService, PlayerService playerService, GameLoop gameLoop)
        {
            _gameService = gameService;
            _messageService = messageService;
            _playerService = playerService;
            _gameLoop = gameLoop;
        }
        public async Task ReceivedDirection(SharedLibs.PacmanMovement movement)
        {
            Console.WriteLine(movement.Direction.ToString());
             _messageService.StorePlayerInput(Context.ConnectionId, movement);
        }
        public override Task OnConnectedAsync()
        {
            var playerid = Context.ConnectionId;
            _playerService.AddPlayer(playerid, new Classes.GameObjects.Player(_gameLoop, _gameService));
            Console.WriteLine("Got a connection from Player ID " + playerid + " !");

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var playerId = Context.ConnectionId;
            _playerService.RemovePlayer(playerId);
            Console.WriteLine("Connection stopped from " + playerId + " !");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
