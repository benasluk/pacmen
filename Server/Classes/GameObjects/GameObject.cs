using Server.Classes.GameLogic;
using Server.Classes.Services;
using SharedLibs;

namespace Server
{
    public abstract class GameObject //Update constructor logic or something
    {
        protected int ID;
        protected int col;
        protected int row;
        protected Direction direction = Direction.Up;
        protected readonly GameLoop _gameLoop;
        protected readonly GameService _gameService;
        
        public GameObject(GameLoop gameLoop, GameService gameService)
        {
            _gameLoop = gameLoop;
            _gameService = gameService;
        }
        public virtual void Destroy()
        {
        }
        public abstract void HandleMovement();

        public void UpdateDirection(Direction newDirection)
        {
            this.direction = newDirection;
        }
        public Direction GetDirection()
        {
            return direction;
        }
        protected GameService GetGameService()
        {
            return _gameService;
        }

        public (int col, int row) GetCurrentLocation()
        {
            return (col, row);
        }
    }
}
