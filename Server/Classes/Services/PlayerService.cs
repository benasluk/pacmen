using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services.Factory;
using Server.Classes.Services.Observer;
using SharedLibs;

namespace Server.Classes.Services
{
    public class PlayerService : IResetabbleLoop
    {
        // #NEW
        private AbstractLevelFactory _levelFactory;
        //private readonly GameLoop _gameLoop;
        private readonly GameService _gameService;
        private GameLoop _gameLoop;

        private Dictionary<string, Player> _players = new Dictionary<string, Player>();

        // #NEW
        public PlayerService(GameService gameService)
        {
            _gameService = gameService;
            ((IResetabbleLoop)this).SubscriberToLevelChange();
        }

        public void SetPlayerFactory(AbstractLevelFactory levelFactory)
        {
            _levelFactory = levelFactory;
        }
        public string GetBackgroundName()
        {
            return _players.FirstOrDefault().Value?.color;
        }
        public Player GetPlayerById(string playerId)
        {
            Player player = null;
            Console.WriteLine("Player ID is " + playerId);
            if (_players.TryGetValue(playerId, out player))
            {
                return player;
            }
            //Console.WriteLine("Returning player id as null");
            return null;
        }
        public void AddPlayer(string playerId, GameLoop gameLoop)
        {
            // #NEW
            Player player = _levelFactory.CreatePacman(gameLoop, _gameService);
            var dictionaryCount = _players.Count;
            player.pacmanNo = (TileStatus)(dictionaryCount + 5);
            switch (player.pacmanNo)
            {
                case TileStatus.Pacman1:
                    player.SetXY(1, 4);
                    break;
                case TileStatus.Pacman2:
                    player.SetXY(26, 4);
                    break;
                case TileStatus.Pacman3:
                    player.SetXY(1, 32);
                    break;
                case TileStatus.Pacman4:
                    player.SetXY(26, 32);
                    break;
            }
            if (_gameLoop is null)
            {
                _gameLoop = gameLoop;
            }
            _players.Add(playerId, player);
        }
        private void ResetPlayers()
        {
            int index = 0;
            string[] playerId = new string[_players.Count];
            foreach (var player in _players)
            {
                playerId[index] = player.Key;
                player.Value.Destroy();
            }
            _players.Clear();
            //Console.WriteLine("lenght is " + playerId.Length);
            for (int i = 0; i < playerId.Length; i++)
            {
                AddPlayer(playerId[i], _gameLoop);
            }
        }
        public void RemovePlayer(string playerId)
        {
            if (_players.TryGetValue(playerId, out Player player))
            {
                player.Destroy();
            }

            _players.Remove(playerId);
        }

        public void UpdatePlayerLocation(PacmanMovement input)
        {
            var player = GetPlayerById(input.PlayerId);
            player.UpdateDirection(input.Direction);
            player.HandleMovement();
        }

        public (int x, int y) GetPlayerCoordinates(string playerId)
        {
            var player = GetPlayerById(playerId);
            return player.GetCurrentLocation();
        }

        public int GetPlayerCount()
        {
            return _players.Count;
        }

        // #NEW
        public void UpdatePlayers(GameLoop gameLoop)
        {
            foreach (var (playerId, _) in _players)
            {
                Player player = _levelFactory.CreatePacman(gameLoop, _gameService);
                _players[playerId] = player;
            }
        }
        public void ResetAfterLevelChange()
        {
            ResetPlayers();
        }
    }
}
