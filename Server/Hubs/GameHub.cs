using Microsoft.AspNetCore.SignalR;
using Server.Classes.Services;
using SharedLibs;
namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly MessageService _mesageService;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        public GameHub (MessageService messageService, GameService gameService, PlayerService playerService)
        {
            _gameService = gameService;
            _mesageService = messageService;
            _playerService = playerService;
        }
        public async Task ReceivedDirection(SharedLibs.PacmanMovement movement)
        {
             _mesageService.StorePlayerInput(Context.ConnectionId, movement);
        }
        public override Task OnConnectedAsync()
        {
            var playerid = Context.ConnectionId;
            _playerService.AddPlayer(playerid, new Classes.GameObjects.Player());
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var playerId = Context.ConnectionId;
            _playerService.RemovePlayer(playerId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
