using Server.GameWorld;

namespace Server.Classes.Services
{
    public class GameService
    {
        private readonly PlayerService _playerService;
        private GameMap _gameMap;
        public GameService(PlayerService playerSerivce) {
            _playerService = playerSerivce;
            _gameMap = new GameMap(28,36);
        }
    }
}
