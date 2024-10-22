using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.Services.Strategy;

public class BFSStrategy : MovementStrategy
{
    public override Direction FindMovementDirection(GameMap gameMap, int currRow, int currCol)
    {
        Queue<(int yc, int xc, List<(int, int)> path)> queue = new();
        queue.Enqueue((currRow, currCol, new List<(int, int)>{(currRow, currCol)}));

        bool[,] visited = new bool[36, 28];
        visited[currRow, currCol] = true;

        while (queue.Count > 0)
        {
            var (currentY, currentX, path) = queue.Dequeue();

            if (IsPacman(currentX, currentY, gameMap))
            {
                (int projectedY, int projectedX) = path.First();

                switch (projectedX - currCol)
                {
                    case > 0:
                        return Direction.Right;
                    case < 0:
                        return Direction.Left;
                }

                switch (projectedY - currRow)
                {
                    case > 0:
                        return Direction.Up;
                    case < 0:
                        return Direction.Down;
                }
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
                    var newPath = new List<(int, int)>(path) { (newY, newX) };
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