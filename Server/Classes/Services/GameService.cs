using Server.Classes.Services.Memento;
using Server.Classes.Services.Observer;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services
{
    public class GameService : IResetabbleLoop
    {
        private GameMap _gameMap;
        string _pauser;
        string adminId;
        public bool paused { get; private set; }
        public MapCaretaker _caretaker = new MapCaretaker();
        public GameMap GetGameMap()
        {
            return _gameMap;
        }
        public bool IsMapFinished()
        {
            return _gameMap.IsFinished();
        }
        public void SetGameMap(GameMap map)
        {
            _gameMap = map;
        }
        public void RestartMap()
        {
            _gameMap.RestartMap();
        }
        public void HandleMapAddon(Addon addon)
        {
            _gameMap.UpdateAddon(addon);
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
        public void SetAdmin(string id)
        {
            adminId = id;
        }
        public string GetAdmin() 
        {
            return adminId;        
        }

        public bool Unpause(string playerId)
        {
            if (playerId.Equals(_pauser) || playerId == adminId)
            {
                paused = false;
                _pauser = null;
                return true;
            }
            else return false;
        }

        public void SaveMap()
        {
            var originator = new MapOriginator();
            originator.SetMap(_gameMap);
            IMapMemento mapState = originator.SaveMapState();
            _caretaker.SaveState(mapState);
        }

        public void RestoreMap()
        {
            var state = _caretaker.RestoreState();
            if (state is not null)
            {
                var originator = new MapOriginator();
                originator.RestoreMap(state);
                SetGameMap(originator.GetMap());
            }
        }
    }
}
