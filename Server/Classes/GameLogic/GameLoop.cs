using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using Server.Classes.Services.Factory;
using Server.Classes.Services.Logging;
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
        public int gameTimer { get; private set; }
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
        private Ilogger textLogger;
        private Ilogger databaseLogger;

        // 0 - Not started
        // 1 - Playing
        // 2 - Paused
        // 3 - Finished
        private int State = 0;

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
            textLogger = new TextFileLogger();
            databaseLogger = new DatabaseWriter(new DatabaseLoggerToWriterAdapter(new DatabaseLogger()));

        }
        public void Start()
        {
            State = 1;
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
            State = 1;
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
        public async void Update(object state)
        {
            State = _commandHandler.HandleMessages(State);
            if(State == 1) {
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
                    if (_playerService.GetPlayerCount() > 0)
                    {
                        Positions test = updateMapInClient();
                        //Console.WriteLine("Sending new map status to " + _playerService.GetPlayerCount() + " player(s)");
                        textLogger.Log("Sending new map status to " + _playerService.GetPlayerCount() + " player(s)");
                        databaseLogger.Log("Sending new map status to " + _playerService.GetPlayerCount() + " player(s)");
                        if (levelRestarted)
                        {
                            test.PlayerColors = new string[1];
                            test.PlayerColors[0] = _playerService.GetBackgroundName();
                            test.ItemIcon = new string[1];
                            test.ItemIcon[0] = ItemList.FirstOrDefault()?.Icon ?? " ";
                            test.SceneChange = true;
                            levelRestarted = false;
                        }
                        try
                        {

                            var testSend = _hubContext.Clients.All.SendAsync("ReceiveMap", test);
                            var dummy = JsonConvert.SerializeObject(test);
                            //await Console.Out.WriteLineAsync(dummy);
                            await testSend;
                        }
                        catch(Exception ex) {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (_movementTimerService.PacmanCanMove())
                    {
                        HandlePacmanMovement();
                    }
                    //if (_movementTimerService.EnemyCanMove())
                    //{
                    //    HandleGhostMovement();
                    //}
                    if (_movementTimerService.EnemyCanMove())
                    {
                        _ghostService.UpdateGhostsLocations();
                    }

                    if(_gameService.IsMapFinished())
                    {
                        State = 3;
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
                _playerService.SetDirection(input.Value.PlayerId, input.Value.Direction);
                //_playerService.UpdatePlayerLocation(input.Value);
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
            //Console.WriteLine(_levelFactory.GetType());
            textLogger.LogMap(map.GetAllTiles().Grid);
            databaseLogger.LogMap(map.GetAllTiles().Grid);
            _gameService.SetGameMap(map);

        }
    }
}
