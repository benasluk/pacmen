using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Strategy;

public class BFSStrategy : MovementStrategy
{
    public override Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol)
    {
        Queue<(int yc, int xc, LinkedList<(int, int)> path)> queue = new();

        var startingPath = new LinkedList<(int, int)>();
        startingPath.AddLast((currRow, currCol));
        queue.Enqueue((currRow, currCol, startingPath));

        bool[,] visited = new bool[36, 28];
        visited[currRow, currCol] = true;

        while (queue.Count > 0)
        {
            var (currentY, currentX, path) = queue.Dequeue();

            if (IsPacman(currentX, currentY, gameMap))
            {
                path.RemoveFirst();

                if (path.Count == 0)
                    return Direction.None;

                (int projectedY, int projectedX) = path.First!.ValueRef;

                if (currRow < projectedY && ValidMove(gameMap, currCol, currRow + 1)) return Direction.Down;
                if (currRow > projectedY && ValidMove(gameMap, currCol, currRow - 1)) return Direction.Up;
                if (currCol < projectedX && ValidMove(gameMap, currCol + 1, currRow)) return Direction.Right;
                if (currCol > projectedX && ValidMove(gameMap, currCol - 1, currRow)) return Direction.Left;
            }

            for (int i = 1; i < 5; i++)
            {
                int newX = currentX;
                int newY = currentY;

                switch ((Direction)i)
                {
                    case Direction.Up:
                        newY += 1;
                        break;
                    case Direction.Down:
                        newY -= 1;
                        break;
                    case Direction.Left:
                        newX -= 1;
                        break;
                    case Direction.Right:
                        newX += 1;
                        break;
                }

                if (IsInBounds(newY, newX)
                    && !visited[newY, newX]
                    && !gameMap.GetTileStatus(newY, newX).Equals(TileStatus.Wall))
                {
                    visited[newY, newX] = true;
                    var newPath = new LinkedList<(int, int)>(path);
                    newPath.AddLast((newY, newX));
                    queue.Enqueue((newY, newX, newPath));
                }
            }
        }

        return Direction.None;
    }

    public override object Clone()
    {
        return new BFSStrategy();
    }
}