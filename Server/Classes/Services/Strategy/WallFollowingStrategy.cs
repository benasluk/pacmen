using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Strategy;

public class WallFollowingStrategy : MovementStrategy
{
    private Direction _currentDirection = Direction.Right;

    public override Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol)
    {
        // Check if the current direction is blocked or out of bounds
        if (IsWallOrOutOfBounds(currRow, currCol, _currentDirection, gameMap))
        {
            // Try to turn right (clockwise direction change)
            _currentDirection = TurnRight(_currentDirection);
        }

        // Check if we can keep moving in the new direction
        if (IsWallOrOutOfBounds(currRow, currCol, _currentDirection, gameMap))
        {
            // Try turning right again if still blocked
            _currentDirection = TurnRight(_currentDirection);
        }

        return _currentDirection;
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
