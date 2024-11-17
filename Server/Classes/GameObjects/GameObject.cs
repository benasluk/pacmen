using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using SharedLibs;

namespace Server
{
    public abstract class GameObject : UnityObject
    {
        protected readonly GameLoop _gameLoop;
        protected readonly GameService _gameService;
        
        public GameObject(GameLoop gameLoop, GameService gameService)
        {
            _gameLoop = gameLoop;
            _gameService = gameService;
        }

        public sealed override void UpdateDirection(Direction newDirection)
        {
            this.direction = newDirection;
        }

        protected GameService GetGameService()
        {
            return _gameService;
        }

        public sealed override (int col, int row) GetCurrentLocation()
        {
            return (col, row);
        }
    }
}
