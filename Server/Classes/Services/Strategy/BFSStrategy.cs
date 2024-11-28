using Server.Classes.Services.Iterator;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Strategy;

public class BFSStrategy : MovementStrategy
{
    public override Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol)
    {
        Queue<(int yc, int xc, LinkedList<(int, int)> path)> queue = new();
        Iterator<(int yc, int xc, LinkedList<(int, int)> path)> queueIterator =
            new QueueIterator<(int yc, int xc, LinkedList<(int, int)> path)>(queue);

        var startingPath = new LinkedList<(int, int)>();
        startingPath.AddLast((currRow, currCol));
        queue.Enqueue((currRow, currCol, startingPath));

        bool[,] visited = new bool[36, 28];
        visited[currRow, currCol] = true;

        while (queueIterator.HasNext())
        {
            var (currentY, currentX, path) = queueIterator.Next();

            Iterator<(int, int)> linkedListIterator = new LinkedListIterator<(int, int)>(path);

            if (IsPacman(currentX, currentY, gameMap))
            {
                if (linkedListIterator.HasNext())
                {
                    linkedListIterator.Next();
                    linkedListIterator.RemoveCurrent();
                }

                if (!linkedListIterator.HasNext())
                    return Direction.None;

                var (projectedY, projectedX) = linkedListIterator.Next();

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