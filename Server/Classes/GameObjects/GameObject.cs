using Server.Classes.GameLogic;
using SharedLibs;

namespace Server
{
    public abstract class GameObject //Update constructor logic or something
    {
        int ID;
        int x;
        int y;
        Direction direction = Direction.None;
        GameLoop _gameLoop;
        public GameObject(GameLoop gameLoop)
        {
            _gameLoop = gameLoop;
            _gameLoop.Movevement += HandleMovement;
        }
        public void Destroy()
        {
            _gameLoop.Movevement -= HandleMovement;
        }
        public abstract void HandleMovement();
    }
}
