using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using Server.Classes.Services.Factory;
using Server.Classes.Services.Observer;
using Server.GameWorld;
using Server.Hubs;
using SharedLibs;
using static Server.Classes.GameLogic.GameLoop;

namespace Server.Classes.GameLogic
{
    public class GameLoop
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GhostService _ghostService;
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
        public delegate void LevelRestart();
        public event LevelRestart LevelRestartEvent;
        private bool levelRestarted = false;
        private readonly CommandHandler _commandHandler;

        private AbstractLevelFactory _levelFactory;
        private List<Item> ItemList;

        private int gameSpeed = 1000 / 10;

        public GameLoop(GameService gameService, PlayerService playerService, MessageService messageService, IHubContext<GameHub> hubContext, GhostService ghostService, CommandHandler commandHandler)
        {
            _gameService = gameService;
            _playerService = playerService;
            _messageService = messageService;
            _hubContext = hubContext;
            _ghostService = ghostService;
            _movementTimerService = MovementTimerServiceSingleton.getInstance();
            _commandHandler = commandHandler;
            gameTimer = 0;
            ItemList = new List<Item>();
        }
        public void Start()
        {
            _timer = new Timer(Update, null, 0, gameSpeed);

            int whatLevel = 0; // new Random(DateTime.Now.Millisecond).Next() % 2;
            if (whatLevel % 2 == 0) _levelFactory = new LevelOneFactory();
            else _levelFactory = new LevelTwoFactory();
            _playerService.SetPlayerFactory(_levelFactory);
            ItemList = _levelFactory.CreateItems(this, _gameService);
            levelRestarted = true;
            LoadLevelMap();
            _ghostService.AddGhosts(this);
        }
        public void RestartLoop()
        {
            ItemList = new List<Item>();
            int level = _messageService.GetLevel();
            if (level % 2 == 0) _levelFactory = new LevelOneFactory();
            else _levelFactory = new LevelTwoFactory();
            Console.WriteLine("Current level set to " +  level);
            _playerService.SetPlayerFactory(_levelFactory);
            LevelRestartEvent?.Invoke();
            levelRestarted = true;
            ResetEvent.ResetLoop();
            LoadLevelMap();
            //Console.WriteLine("Done restarting");
        }
        public void Update(object state)
        {
            _commandHandler.HandleMessages();
            if(!_gameService.paused) {
                if (_playerService.GetPlayerCount() >= 1)
                {
                    _movementTimerService.UpdateElapsedTime(gameSpeed);
                    gameTimer += gameSpeed;
                    _hubContext.Clients.All.SendAsync("UpdateTimer", gameTimer);
                    HandlePlayerInputs();
                    if (CheckForLevelChange())
                    {
                        Console.WriteLine("Restarting loop");
                        RestartLoop();
                        _messageService.ResetLevel();
                    }
                    //if (_movementTimerService.PacmanCanMove())
                    //{
                    //    HandlePacmanMovement();
                    //}
                    //if (_movementTimerService.EnemyCanMove())
                    //{
                    //    HandleGhostMovement();
                    //}
                    if (_movementTimerService.EnemyCanMove())
                    {
                        _ghostService.UpdateGhostsLocations();
                    }

                    if (_playerService.GetPlayerCount() > 0)
                    {
                        Positions test = updateMapInClient();
                        //Console.WriteLine("Sending new map status to " + _playerService.GetPlayerCount() + " player(s)");
                        if (levelRestarted)
                        {
                            test.PlayerColors = new string[1];
                            test.PlayerColors[0] = _playerService.GetBackgroundName();
                            test.ItemIcon = new string[1];
                            test.ItemIcon[0] = ItemList.FirstOrDefault()?.Icon ?? " ";
                            test.SceneChange = true;
                            levelRestarted = false;
                        }
                        _hubContext.Clients.All.SendAsync("ReceiveMap", test);
                    }
                }
            }
        }
        private bool CheckForLevelChange()
        {
            return _messageService.IsLevelChange();
        }
        private void HandlePlayerInputs()
        {
            //Console.WriteLine("Handling inputs");
            var inputs = _messageService.GetPlayerInputs();
            foreach (var input in inputs)
            {
                _playerService.UpdatePlayerLocation(input.Value);
                //(int currentX, int currentY) = _playerService.GetPlayerCoordinates(input.Key);
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
            Positions resultObj = _gameService.GetGameMap().GetAllTiles();
            resultObj.Scores = PlayerScoreSingleton.getInstance().GetScore();
            return resultObj;
        }
        public void RestartTimer()
        {
            gameTimer = 0;
        }

        private void LoadLevelMap()
        {
            GameMap map = _levelFactory.CreateMap();
            Console.WriteLine(_levelFactory.GetType());
            _gameService.SetGameMap(map);
        }
    }
}
