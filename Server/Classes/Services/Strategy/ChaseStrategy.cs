using Server.GameWorld;
using SharedLibs;
using System;

namespace Server.Classes.Services.Strategy
{
    public class RandomChaseStrategy : MovementStrategy
    {
        private const int ChaseRadius = 5;
        private static readonly Random Random = new Random();

        public override Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol)
        {
            if (IsPacmanNearby(currRow, currCol, gameMap, out (int pacmanRow, int pacmanCol) pacmanPosition))
            {
                return GetDirectionTowards(currRow, currCol, pacmanPosition.pacmanRow, pacmanPosition.pacmanCol);
            }

            return GetRandomDirection(currRow, currCol, gameMap);
        }

        private bool IsPacmanNearby(int currRow, int currCol, GameMap gameMap, out (int pacmanRow, int pacmanCol) pacmanPosition)
        {
            pacmanPosition = (currRow, currCol);
            (int mapRows, int mapCol) = gameMap.GetMapSize();

            for (int y = Math.Max(0, currRow - ChaseRadius); y <= Math.Min(mapRows- 1, currRow + ChaseRadius); y++)
            {
                for (int x = Math.Max(0, currCol - ChaseRadius); x <= Math.Min(mapCol - 1, currCol + ChaseRadius); x++)
                {
                    if (IsPacman(x, y, gameMap))
                    {
                        pacmanPosition = (y, x);
                        return true;
                    }
                }
            }

            return false;
        }

        public Direction GetRandomDirection(int currRow, int currCol, GameMap gameMap)
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

        private Direction GetDirectionTowards(int currRow, int currCol, int targetRow, int targetCol)
        {
            if (currRow < targetRow) return Direction.Down;
            if (currRow > targetRow) return Direction.Up;
            if (currCol < targetCol) return Direction.Right;
            if (currCol > targetCol) return Direction.Left;

            return Direction.None;
        }
    }
}
