using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services.Factory;
using SharedLibs;

namespace Server.Classes.Services
{
    public class PlayerService 
    {
        private readonly GameService _gameService;
        private GameLoop _gameLoop;

        private Dictionary<string, Player> _players = new Dictionary<string, Player>();

        public PlayerService(GameService gameService)
        {
            _gameService = gameService;
        }

        public string GetBackgroundName()
        {
            return _players.FirstOrDefault().Value?.color;
        }
        public Player GetPlayerById(string playerId)
        {
            Player player = null;
            Console.WriteLine("Player ID is " + playerId);
            if (_players.TryGetValue(playerId, out player))
            {
                return player;
            }
            return null;
        }
        public void RemovePlayer(string playerId)
        {
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

        public virtual int GetPlayerCount()
        {
            return _players.Count;
        }

    }
}
