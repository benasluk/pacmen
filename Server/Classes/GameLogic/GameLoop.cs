using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using Server.Classes.Services.Factory;
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
        private readonly MovementTimerServiceSingleton _movementTimerService;
        private Timer _timer;
        public delegate void PacmanMovevementHandler();
        public event PacmanMovevementHandler PacmanMovevement;
        public delegate void GhostMovevementHandler();
        public event GhostMovevementHandler GhostMovevement;
        private bool _gameStarted = false;

        // #NEW
        private AbstractLevelFactory _levelFactory;
        private List<Item> ItemList;
        
        public GameLoop(GameService gameService, PlayerService playerService, MessageService messageService, IHubContext<GameHub> hubContext)
        {
            _gameService = gameService;
            _playerService = playerService;
            _messageService = messageService;
            _hubContext = hubContext;
            _movementTimerService = MovementTimerServiceSingleton.getInstance();
            // #NEW
            ItemList = new List<Item>();
        }
        public void Start()
        {
            _timer = new Timer(Update, null, 0, 1000 / 60);
            
            // #NEW
            _levelFactory = new LevelOneFactory();
            _playerService.SetPlayerFactory(_levelFactory);
            ItemList = _levelFactory.CreateItems(this, _gameService);
            LoadLevelMap();
        }
        public void Update(object state)
        {
            if (_playerService.GetPlayerCount() >= 2)
            {
                _movementTimerService.UpdateElapsedTime(1000 / 60);
                HandlePlayerInputs();
                if (_movementTimerService.PacmanCanMove())
                {
                    HandlePacmanMovement();
                }
                //if (_movementTimerService.EnemyCanMove())
                //{
                //    HandleGhostMovement();
                //}

                // #NEW
                // if (onInitialLevel2)
                // {
                //     _levelFactory = new LevelTwoFactory();
                //     ItemList = _levelFactory.CreateItems(this, _gameService);
                //     _playerService.UpdatePlayers();
                //     LoadLevelMap();
                // }

                if (_playerService.GetPlayerCount() > 0)
                {
                    Positions test = updateMapInClient();
                    Console.WriteLine("Sending new map status to " + _playerService.GetPlayerCount() + " player(s)");
                    _hubContext.Clients.All.SendAsync("ReceiveMap", test);
                    _hubContext.Clients.All.SendAsync("Test", "Hello from c#");
                }
            }
        }
        private void HandlePlayerInputs()
        {
            var inputs = _messageService.GetPlayerInputs();
            foreach (var input in inputs)
            {
                _playerService.UpdatePlayerLocation(input.Value);
                (int currentX, int currentY) = _playerService.GetPlayerCoordinates(input.Key);
            }
        }
        private void HandlePacmanMovement()
        {
            PacmanMovevement?.Invoke();
        }
        private void HandleGhostMovement()
        {
            GhostMovevement?.Invoke();
        }
        public Positions updateMapInClient()
        {
            return _gameService.GetGameMap().GetAllTiles();
        }

        // #NEW
        private void LoadLevelMap()
        {
            GameMap map = _levelFactory.CreateMap();
            _gameService.SetGameMap(map);
        }
    }
}
