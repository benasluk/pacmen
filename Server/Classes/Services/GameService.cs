using Server.Classes.Services.Observer;
using Server.GameWorld;

namespace Server.Classes.Services
{
    public class GameService : IResetabbleLoop
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
        public void RestartMap()
        {
            _gameMap.RestartMap();
        }

        public void ResetAfterLevelChange()
        {
            RestartMap();
        }
    }
}
