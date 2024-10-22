using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Strategy;

public abstract class MovementStrategy : ICloneable
{
    protected static bool IsInBounds(int y, int x)
    {
        return x is >= 0 and < 28 && y is >= 0 and < 36;
    }

    protected static bool IsPacman(int x, int y, GameMap map)
    {
        var tileStatus = map.GetTileStatus(y, x);
        return tileStatus.Equals(TileStatus.Pacman1)
               || tileStatus.Equals(TileStatus.Pacman2)
               || tileStatus.Equals(TileStatus.Pacman3)
               || tileStatus.Equals(TileStatus.Pacman4);
    }
    
    protected static bool ValidMove(GameMap map, int x, int y)
    {
        var tile = map.GetTileStatus(y, x);
        return !tile.Equals(TileStatus.Wall);
    }

    public abstract Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol);
    public abstract object Clone();
}