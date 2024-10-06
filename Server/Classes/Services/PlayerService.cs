using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services.Factory;
using SharedLibs;

namespace Server.Classes.Services
{
    public class PlayerService
    {
        // #NEW
        private AbstractLevelFactory _levelFactory;
        //private readonly GameLoop _gameLoop;
        private readonly GameService _gameService;
        
        private Dictionary<string, Player> _players = new Dictionary<string, Player>();

        // #NEW
        public PlayerService(GameService gameService)
        {
            _gameService = gameService;
        }

        public void SetPlayerFactory(AbstractLevelFactory levelFactory) 
        { 
            _levelFactory = levelFactory;
        }

        public Player GetPlayerById(string playerId)
        {
            Player player = null;
            if (_players.TryGetValue(playerId, out player))
            {
                return player;
            }
            return null;
        }
        public void AddPlayer (string playerId, GameLoop gameLoop)
        {
            // #NEW
            Player player = _levelFactory.CreatePacman(gameLoop, _gameService);
            var dictionaryCount = _players.Count;
            player.pacmanNo = (TileStatus)(dictionaryCount + 5);
            
            _players.Add(playerId, player);
        }
        public void RemovePlayer (string playerId) { 
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
    }
}
