using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using Server.GameWorld;
using Server.Hubs;
using SharedLibs;
using static Server.Classes.GameLogic.GameLoop;

namespace Server.Classes.GameLogic
{
    public class GameLoop
    {
        private readonly GameService _gameService;
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
        public bool levelRestarted = false;

        private List<Item> ItemList;

        private int gameSpeed = 1000 / 10;

        public GameLoop(GameService gameService, MessageService messageService, IHubContext<GameHub> hubContext)
        {
            _gameService = gameService;
            _messageService = messageService;
            _hubContext = hubContext;
            _movementTimerService = MovementTimerServiceSingleton.getInstance();
            gameTimer = 0;
            ItemList = new List<Item>();
            levelRestarted = false;
        }
        public void Start()
        {

            int whatLevel = 0; 
            levelRestarted = true;
        }
        public void RestartLoop()
        {
            ItemList = new List<Item>();
            int level = _messageService.GetLevel();
            LevelRestartEvent?.Invoke();
            levelRestarted = true;
        }
        private bool CheckForLevelChange()
        {
            return _messageService.IsLevelChange();
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
            var resultObj = _gameService.GetGameMap().GetAllTiles();
            return resultObj;
        }
        public void RestartTimer()
        {
            gameTimer = 0;
        }

    }
}
