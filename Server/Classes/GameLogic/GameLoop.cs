﻿using Microsoft.AspNetCore.SignalR;
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
        private int gameTimer;
        public delegate void PacmanMovevementHandler();
        public event PacmanMovevementHandler PacmanMovevement;
        public delegate void GhostMovevementHandler();
        public event GhostMovevementHandler GhostMovevement;
        private bool _gameStarted = false;

        // #NEW
        private AbstractLevelFactory _levelFactory;
        private List<Item> ItemList;

        private int gameSpeed = 1000 / 10;
        
        public GameLoop(GameService gameService, PlayerService playerService, MessageService messageService, IHubContext<GameHub> hubContext)
        {
            _gameService = gameService;
            _playerService = playerService;
            _messageService = messageService;
            _hubContext = hubContext;
            _movementTimerService = MovementTimerServiceSingleton.getInstance();
            gameTimer = 0;
            // #NEW
            ItemList = new List<Item>();
        }
        public void Start()
        {
            _timer = new Timer(Update, null, 0, gameSpeed);

            // #NEW

            int whatLevel = new Random(DateTime.Now.Millisecond).Next() % 2;
            if (whatLevel % 2 == 0) _levelFactory = new LevelOneFactory();
            else _levelFactory = new LevelTwoFactory();
            _playerService.SetPlayerFactory(_levelFactory);
            ItemList = _levelFactory.CreateItems(this, _gameService);
            LoadLevelMap();
        }
        public void Update(object state)
        {
            if (_playerService.GetPlayerCount() >= 1)
            {
                _movementTimerService.UpdateElapsedTime(gameSpeed);
                gameTimer += gameSpeed;
                _hubContext.Clients.All.SendAsync("UpdateTimer", gameTimer);
                HandlePlayerInputs();
                //if (_movementTimerService.PacmanCanMove())
                //{
                //    HandlePacmanMovement();
                //}
                //if (_movementTimerService.EnemyCanMove())
                //{
                //    HandleGhostMovement();
                //}

                // #NEW
                // if (onInitialLevel2)
                // {
                //     _levelFactory = new LevelTwoFactory();
                //     ItemList = _levelFactory.CreateItems(this, _gameService);
                //     _playerService.SetPlayerFactory(_levelFactory);
                //     _playerService.UpdatePlayers(this);
                //     LoadLevelMap();
                // }

                if (_playerService.GetPlayerCount() > 0)
                {
                    Positions test = updateMapInClient();
                    Console.WriteLine("Sending new map status to " + _playerService.GetPlayerCount() + " player(s)");
                    _hubContext.Clients.All.SendAsync("ReceiveMap", test);
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
        public void RestartTimer()
        {
            gameTimer = 0;
        }

        // #NEW
        private void LoadLevelMap()
        {
            GameMap map = _levelFactory.CreateMap();
            _gameService.SetGameMap(map);
        }
    }
}
