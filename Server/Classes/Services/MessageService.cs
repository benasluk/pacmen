using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services
{
    public class MessageService
    {
        private readonly Dictionary<string, PacmanMovement> _playerInputs = new Dictionary<string, PacmanMovement>();
        private GameMap _gameMap = null;
        public MessageService(GameService gameService, PlayerService playerService) {

        }

        public void StorePlayerInput(string playerId, PacmanMovement input)
        {
            _playerInputs.Add(playerId, input);
        }
        public Dictionary<string, PacmanMovement> GetPlayerInputs()
        {
            var inputs = new Dictionary<string, PacmanMovement>(_playerInputs);
            _playerInputs.Clear();
            return _playerInputs;
        }
        public GameMap StoreMap(GameMap map)
        {
            _gameMap = map;
            return _gameMap;
        }
    }
}
