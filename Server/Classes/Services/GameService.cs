using Server.GameWorld;

namespace Server.Classes.Services
{
    public class GameService
    {
        private GameMap _gameMap;

        public GameMap GetGameMap()
        {
            return _gameMap;
        }

        // #NEW
        public void SetGameMap(GameMap map)
        {
            _gameMap = map;
        }
    }
}
