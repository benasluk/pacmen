using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Strategy;

public class RandomStrategy : MovementStrategy
{
    private static readonly Random Random = new Random();
    public override Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol)
    {
        int newY = currRow;
        int newX = currCol;
        Direction chosenDirection;
        
        do
        {
            chosenDirection = (Direction)Random.Next(1, 5);

            switch (chosenDirection)
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
        }
        while (!IsInBounds(newX, newY) || gameMap.GetTileStatus(newY, newX).Equals(TileStatus.Wall));

        return chosenDirection;
    }
}