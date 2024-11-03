using Microsoft.AspNetCore.SignalR;
using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;
using System.Diagnostics;
using Server.Classes.Services.Command;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly MessageService _messageService;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GameLoop _gameLoop;
        public GameHub(MessageService messageService, GameService gameService, PlayerService playerService, GameLoop gameLoop)
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
            _messageService.StorePlayerInput(Context.ConnectionId, movement);
        }
        public async Task ReceiveCommand(CommandType type, CommandAction action)
        {
            switch (type)
            {
                case CommandType.Pause:
                    _messageService.StoreCommand(new PauseCommand(_gameService, this),action, Context.ConnectionId);
                    break;
            }
        }
        public async Task UpdateGameMapAddons(Type addonType, string value)
        {
            Console.WriteLine("Received Addon Update");

            if (Activator.CreateInstance(addonType) is Addon addonToUpdate)
            {
                Console.WriteLine("Going to gameservice");
                _gameService.HandleMapAddon(addonToUpdate);
            }
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
        public async Task Handshake(HandShake handshake)
        {

            if (string.IsNullOrEmpty(handshake.PlayerName) || handshake.PlayerName.Length < 4)
            {
                await Clients.Caller.SendAsync("HandshakeFailed", "Invalid player name.");
                return;

            }
            var playerid = Context.ConnectionId;

            _playerService.AddPlayer(playerid, _gameLoop);

            Clients.All.SendAsync("UpdatePlayerCount", _playerService.GetPlayerCount());


            await Clients.Caller.SendAsync("HandshakeReceived", $"Welcome, {handshake.PlayerName}");
            var thisPlayerPacman = _playerService.GetPlayerById(playerid).pacmanNo;
            await Clients.Caller.SendAsync("ReceivePacman", thisPlayerPacman);
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var playerId = Context.ConnectionId;
            _playerService.RemovePlayer(playerId);
            Console.WriteLine("Connection stopped from " + playerId + " !");
            Clients.All.SendAsync("UpdatePlayerCount", _playerService.GetPlayerCount());
            if (_playerService.GetPlayerCount() == 0)
            {
                _gameService.RestartMap();
                _gameLoop.RestartTimer();
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
