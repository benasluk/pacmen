using Server.Classes.Services.Bridge;
using SharedLibs;

namespace Server.Classes.Services.Chain_Of_Command
{
    public class CollsionHealthHandler : CollisionHandler
    {
        public override void HandleCollision(CollisionEvent collision)
        {
            if (collision.CollidedTile == TileStatus.Ghost || collision.CollidedTile == TileStatus.Ghost1 || collision.CollidedTile == TileStatus.Ghost2 || collision.CollidedTile == TileStatus.Ghost3 || collision.CollidedTile == TileStatus.Ghost4 || collision.CollidedTile == TileStatus.PelletAndGhost) //wtf
            {
                Console.WriteLine("Pacman collided with a Ghost. Reducing health.");
                ServiceLocator.GetService<PlayerService>().RemovePlayerByValue(collision.Player);
            }
            _nextHandler?.HandleCollision(collision);
        }
    }
}
