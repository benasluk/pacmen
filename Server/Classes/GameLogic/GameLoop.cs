using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Server.Classes.Services;
using Server.GameWorld;
using Server.Hubs;
using SharedLibs;

namespace Server.Classes.GameLogic
{
    public class GameLoop
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly MessageService _messageService;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly MovementTimerService _movementTimerService;
        private Timer _timer;
        public delegate void MovevementHandler();
        public event MovevementHandler Movevement;
        private bool _gameStarted = false;
        public GameLoop(GameService gameService, PlayerService playerService, MessageService messageService, IHubContext<GameHub> hubContext, MovementTimerService movementTimerService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _messageService = messageService;
            _hubContext = hubContext;
            _movementTimerService = movementTimerService;
        }
        public void Start()
        {
            _timer = new Timer(Update, null, 0, 1000);
        }
        public void Update(object state)
        {
            // laikinai nera svarbu sitas, nes dabar 1fps, bet kaip bus tarkim 10fps, galesim padaryt kad judetu tik 2 kartus per sec
            _movementTimerService.UpdateElapsedTime(1000);
            if (_movementTimerService.CanMove())
            {
                HandlePlayerInputs();
                HandleObjectMovement();
            }
            
            if (_playerService.GetPlayerCount() > 0)
            {
                Positions test = updateMapInClient();
                Console.WriteLine("Sending new map status to " + _playerService.GetPlayerCount() + " player(s)");
                //Kazkas su serialization blogai, nes apatinis sendasync isivykdo, virsutinis ne:)
                _hubContext.Clients.All.SendAsync("ReceiveMap", test);
                _hubContext.Clients.All.SendAsync("Test", "Hello from c#");
            }
        }
        private void HandlePlayerInputs()
        {
            var inputs = _messageService.GetPlayerInputs();
            foreach (var input in inputs)
            {
                _playerService.UpdatePlayerLocation(input.Value);
                (int currentX, int currentY) = _playerService.GetPlayerCoordinates(input.Key);
                _gameService.GetGameMap().UpdateTile(currentY, currentX, _playerService.GetPlayerById(input.Key).pacmanNo);
            }
        }
        private void HandleObjectMovement()
        {
            Movevement?.Invoke();
        }
        public Positions updateMapInClient()
        {
            return _gameService.GetGameMap().GetAllTiles();
        }

    }
}
