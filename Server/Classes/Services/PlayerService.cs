using Server.Classes.GameObjects;
using SharedLibs;

namespace Server.Classes.Services
{
    public class PlayerService
    {
        private Dictionary<string, Player> _players = new Dictionary<string, Player>();

        public Player GetPlayerById(string playerId)
        {
            Player player = null;
            if (_players.TryGetValue(playerId, out player))
            {
                return player;
            }
            return null;
        }
        public void AddPlayer (string playerId, Player player)
        {
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
    }
}
