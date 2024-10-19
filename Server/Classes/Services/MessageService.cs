using Server.Classes.Services.Observer;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services
{
    public class MessageService : IResetabbleLoop
    {
        private readonly Dictionary<string, PacmanMovement> _playerInputs = new Dictionary<string, PacmanMovement>();
        private GameMap _gameMap = null;
        private object lockObj;
        private object levelChangeLock; //None of the methods using this lock can be accessed if atleast one method using this is locked
        private int newLevel = -1;
        public MessageService(GameService gameService, PlayerService playerService) {
            lockObj = new object();
            levelChangeLock = new object();
            ((IResetabbleLoop)this).SubscriberToLevelChange();

        }
        public void StoreLevelChange(int level)
        {
            lock(levelChangeLock)
            {
                if (newLevel == -1 && level > -1 && level < 2)
                {
                    newLevel = level;
                }
            }
        }
        public void ResetLevel()
        {
            lock(levelChangeLock)
            {
                newLevel = -1;
            }
        }
        public bool IsLevelChange()
        {
            lock(levelChangeLock)
            {
                return newLevel != -1;
            }
        }
        public int GetLevel()
        {
            lock (levelChangeLock)
            {
                return newLevel;
            }
        }
        public void StorePlayerInput(string playerId, PacmanMovement input)
        {
            lock (lockObj)
            {
                _playerInputs[playerId] = input;
            }
        }
        public Dictionary<string, PacmanMovement> GetPlayerInputs()
        {
            Dictionary<string, PacmanMovement> inputs; 
            lock (lockObj)
            {
                inputs = new Dictionary<string, PacmanMovement>(_playerInputs);
                _playerInputs.Clear();
            }
            return inputs;
        }
        public GameMap StoreMap(GameMap map)
        {
            _gameMap = map;
            return _gameMap;
        }

        public void ResetAfterLevelChange()
        {
            newLevel -= 1;
            _playerInputs.Clear();
        }
    }
}
