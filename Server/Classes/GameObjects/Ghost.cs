using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.GameObjects;

public class Ghost : GameObject
{
    public string Color;
    public Ghost(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {

    }

    public override void HandleMovement()
    {
        var gameMap = GetGameService().GetGameMap();
        var newDirection = FindPacManDirection(gameMap);
        UpdateDirection(newDirection);

        int projectedX = col;
        int projectedY = row;

        switch (direction)
        {
            case Direction.None:
                break;
            case Direction.Up:
                projectedY += 1;
                break;
            case Direction.Down:
                projectedY -= 1;
                break;
            case Direction.Left:
                projectedX -= 1;
                break;
            case Direction.Right:
                projectedX += 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (ValidMove(gameMap, projectedY, projectedY))
        {
            col = projectedX;
            row = projectedY;
        }
        
    }

    private readonly Direction[] directions = new Direction[]
    {
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right
    };

    private Direction FindPacManDirection(GameMap gameMap)
    {
        Queue<(int yc, int xc, List<(int, int)> path)> queue = new();
        queue.Enqueue((row, col, new List<(int, int)>{(row, col)}));

        bool[,] visited = new bool[36, 28];
        visited[row, col] = true;

        while (queue.Count > 0)
        {
            var (currentY, currentX, path) = queue.Dequeue();

            if (IsPacman(currentX, currentY, gameMap))
            {
                (int projectedY, int projectedX) = path.First();

                switch (projectedX - col)
                {
                    case > 0:
                        return Direction.Right;
                    case < 0:
                        return Direction.Left;
                }

                switch (projectedY - row)
                {
                    case > 0:
                        return Direction.Up;
                    case < 0:
                        return Direction.Down;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                int newX = currentX;
                int newY = currentY;
                
                switch (directions[i])
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

                if (IsInBounds(newX, newY) 
                    && !visited[newX, newY] 
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

    private static bool IsInBounds(int y, int x)
    {
        return x is >= 0 and < 28 && y is >= 0 and < 36;
    }

    private static bool IsPacman(int x, int y, GameMap map)
    {
        var tileStatus = map.GetTileStatus(y, x);
        return tileStatus.Equals(TileStatus.Pacman1)
               || tileStatus.Equals(TileStatus.Pacman2)
               || tileStatus.Equals(TileStatus.Pacman3)
               || tileStatus.Equals(TileStatus.Pacman4);
    }
    
    private static bool ValidMove(GameMap map, int x, int y)
    {
        var tile = map.GetTileStatus(y, x);
        return !tile.Equals(TileStatus.Wall);
    }
}