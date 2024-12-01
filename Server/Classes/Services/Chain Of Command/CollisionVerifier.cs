using SharedLibs;

namespace Server.Classes.Services.Chain_Of_Command
{
    public class CollisionVerifier : CollisionHandler
    {
        public override void HandleCollision(CollisionEvent collision)
        {
            if (collision.CollidedTile != TileStatus.Empty)
            {
                Console.WriteLine($"Collision detected: {collision.CollidedTile}");
                _nextHandler?.HandleCollision(collision);
            }
            else
            {
                Console.WriteLine("No collision detected. Stopping chain.");
            }
        }
    }
}
