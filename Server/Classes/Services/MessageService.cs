using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services
{
    public class MessageService 
    {
        private readonly Dictionary<string, PacmanMovement> _playerInputs = new Dictionary<string, PacmanMovement>();
        private GameMap _gameMap = null;
        private object lockObj;
        private object levelChangeLock; //None of the methods using this lock can be accessed if atleast one method using this is locked
        private int newLevel = -1;
        public MessageService() {
            lockObj = new object();
            levelChangeLock = new object();
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
        public virtual int GetLevel()
        {
            lock (levelChangeLock)
            {
                return newLevel;
            }
        }
        public virtual void StorePlayerInput(string playerId, PacmanMovement input)
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

        public void ResetAfterLevelChange()
        {
            newLevel -= 1;
            _playerInputs.Clear();
        }
    }
}
