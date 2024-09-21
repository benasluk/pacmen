using Microsoft.AspNetCore.SignalR;
using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
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
            await Console.Out.WriteLineAsync(movement.PlayerId);
            await Console.Out.WriteLineAsync(movement.Direction.ToString());
            await Console.Out.WriteLineAsync(_messageService.ToString());
            _messageService.StorePlayerInput(Context.ConnectionId, movement);
        }
        public override Task OnConnectedAsync()
        {
            var playerid = Context.ConnectionId;
            Console.WriteLine("Got a connection from Player ID " + playerid + " !");

            return base.OnConnectedAsync();
        }
        public async Task Handshake(HandShake handshake)
        {

            if (string.IsNullOrEmpty(handshake.PlayerName) || handshake.PlayerName.Length < 4)
            {
                await Clients.Caller.SendAsync("HandshakeFailed", "Invalid player name.");
                return;

            }
            var playerid = Context.ConnectionId;

            _playerService.AddPlayer(playerid, new Classes.GameObjects.Player(_gameLoop, _gameService));


            await Clients.Caller.SendAsync("HandshakeReceived", $"Welcome, {handshake.PlayerName}");
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
