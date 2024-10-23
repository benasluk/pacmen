using Server.Classes.Services.Observer;
using Server.GameWorld;

namespace Server.Classes.Services
{
    public class GameService : IResetabbleLoop
    {
        private GameMap _gameMap;
        string _pauser;
        public bool paused { get; private set; }
        public GameMap GetGameMap()
        {
            return _gameMap;
        }

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
        public string PausedBy()
        {
            return _pauser;
        }
        public bool Pause(string playerId)
        {
            _pauser = playerId;
            paused = true;
            return true;
        }

        public bool Unpause(string playerId)
        {
            if (playerId.Equals(_pauser))
            {
                paused = false;
                _pauser = null;
                return true;
            }
            else return false;
        }
    }
}
