using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Strategy;

public class WallFollowingStrategy : MovementStrategy
{
    private Direction _currentDirection = Direction.Right;

    public override Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol)
    {
        if (IsWallOrOutOfBounds(currRow, currCol, _currentDirection, gameMap))
        {
            _currentDirection = TurnRight(_currentDirection);
        }

        if (IsWallOrOutOfBounds(currRow, currCol, _currentDirection, gameMap))
        {
            _currentDirection = TurnRight(_currentDirection);
        }

        return _currentDirection;
    }

    public override object Clone()
    {
        return new WallFollowingStrategy();
    }

    private bool IsWallOrOutOfBounds(int currRow, int currCol, Direction direction, GameMap gameMap)
    {
        int newY = currRow, newX = currCol;

        switch (direction)
        {
            case Direction.Up:
                newY -= 1;
                break;
            case Direction.Down:
                newY += 1;
                break;
            case Direction.Left:
                newX -= 1;
                break;
            case Direction.Right:
                newX += 1;
                break;
        }
        
        return !IsInBounds(newX, newY) || gameMap.GetTileStatus(newY, newX).Equals(TileStatus.Wall);
    }

    private Direction TurnRight(Direction currentDirection)
    {
        switch (currentDirection)
        {
            case Direction.Up:
                return Direction.Right;
            case Direction.Right:
                return Direction.Down;
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Up;
            default:
                return Direction.None;
        }
    }
}
