using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Chain_Of_Command
{
    public class CollisionEvent
    {
        public Player Player { get; private set; }
        public TileStatus CollidedTile { get; private set; }
        public GameMap GameMap { get; private set; }
        public GameLoop GameLoop { get; private set; }
        public int col { get; set; }
        public int row { get; set; }

        public CollisionEvent(Player player, TileStatus tile, GameMap gameMap, GameLoop gameLoop, int col, int row)
        {
            Player = player;
            CollidedTile = tile;
            GameMap = gameMap;
            GameLoop = gameLoop;
            this.col = col;
            this.row = row;
        }
    }
}
