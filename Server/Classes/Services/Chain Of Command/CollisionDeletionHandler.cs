using SharedLibs;

namespace Server.Classes.Services.Chain_Of_Command
{
    public class CollisionDeletionHandler : CollisionHandler
    {
        public override void HandleCollision(CollisionEvent collision)
        {
            if (collision.CollidedTile == TileStatus.Pellet || collision.CollidedTile == TileStatus.PelletSmall || collision.CollidedTile == TileStatus.PelletLarge)
            {
                //Console.WriteLine($"Deleting {collision.CollidedTile} from the map.");
                collision.GameMap.UpdateTile(collision.row, collision.col, TileStatus.Empty);
            }

            _nextHandler?.HandleCollision(collision);
        }
    }
}
