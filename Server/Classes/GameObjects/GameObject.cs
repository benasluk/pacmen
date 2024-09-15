using Server.Classes.GameLogic;
using Server.Classes.Services;
using SharedLibs;

namespace Server
{
    public abstract class GameObject //Update constructor logic or something
    {
        protected int ID;
        protected int x;
        protected int y;
        protected Direction direction = Direction.None;
        private readonly GameLoop _gameLoop;
        private readonly GameService _gameService;
        public GameObject(GameLoop gameLoop, GameService gameService)
        {
            _gameLoop = gameLoop;
            _gameService = gameService;
            _gameLoop.Movevement += HandleMovement;
        }
        public void Destroy()
        {
            _gameLoop.Movevement -= HandleMovement;
        }
        public abstract void HandleMovement();

        public abstract void UpdateDirection(Direction newDirection);

        protected GameService GetGameService()
        {
            return _gameService;
        }
    }
}
