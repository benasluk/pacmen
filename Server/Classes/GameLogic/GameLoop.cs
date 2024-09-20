using Microsoft.AspNetCore.SignalR;
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
        private Timer _timer;
        public delegate void MovevementHandler();
        public event MovevementHandler Movevement;
        private bool _gameStarted = false;
        public GameLoop(GameService gameService, PlayerService playerService, MessageService messageService, IHubContext<GameHub> hubContext)
        {
            _gameService = gameService;
            _playerService = playerService;
            _messageService = messageService;
            _hubContext = hubContext;
        }
        public void Start()
        {
            _timer = new Timer(Update, null, 0, 1000);
        }
        public void Update(object state)
        {
            HandlePlayerInputs();
            HandleObjectMovement();
            Positions test = updateMapInClient();
            Console.WriteLine("Sending new map status");
            _hubContext.Clients.All.SendAsync("ReceiveMap", test);
        }
        private void HandlePlayerInputs()
        {
            var inputs = _messageService.GetPlayerInputs();
            foreach (var input in inputs)
            {
                _playerService.UpdatePlayerLocation(input.Value);
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
