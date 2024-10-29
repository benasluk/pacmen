using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Strategy;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.GameObjects;

public class Ghost : GameObject, ICloneable
{
    public string Color;
    public TileStatus ghostNo = TileStatus.Empty;
    private TileStatus lastVisitedTile = TileStatus.Empty;
    private MovementStrategy _movementStrategy;
    public Ghost(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
        _gameLoop.GhostMovevement += HandleMovement;
    }

    public override void Destroy()
    {
        _gameLoop.GhostMovevement -= HandleMovement;
        base.Destroy();
    }

    public override void HandleMovement()
    {
        var gameMap = GetGameService().GetGameMap();
        var newDirection = _movementStrategy.FindMovementDirection(gameMap, row, col);
        UpdateDirection(newDirection);

        int projectedX = col;
        int projectedY = row;

        switch (direction)
        {
            case Direction.None:
                break;
            case Direction.Up:
                projectedY -= 1;
                break;
            case Direction.Down:
                projectedY += 1;
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
        
        if(ValidMove(gameMap, projectedX, projectedY))
        {
            gameMap.UpdateTile(row, col, lastVisitedTile);

            col = projectedX;
            row = projectedY;

            lastVisitedTile = gameMap.UpdateTile(row, col, ghostNo);
        }
    }

    private static bool ValidMove(GameMap map, int x, int y)
    {
        List<TileStatus> invalidTiles = new List<TileStatus>()
        {
            TileStatus.Wall, 
            TileStatus.Ghost1, 
            TileStatus.Ghost2, 
            TileStatus.Ghost3, 
            TileStatus.Ghost4
        };
        var tile = map.GetTileStatus(y, x);
        return !invalidTiles.Contains(tile);
    }

    public void SetMovementStrategy(MovementStrategy movementStrategy)
    {
        _movementStrategy = movementStrategy;
    }

    public void SetCoordinates(int col, int row)
    {
        this.col = col;
        this.row = row;
    }

    // shallow copy
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public Ghost DeepCopy()
    {
        Ghost clonedGhost = new Ghost(_gameLoop, _gameService);

        clonedGhost.Color = Color;
        clonedGhost.ghostNo = ghostNo;
        clonedGhost.SetCoordinates(col, row);
        clonedGhost.UpdateDirection(direction);

        if (_movementStrategy is not null)
        {
            clonedGhost._movementStrategy = (MovementStrategy)_movementStrategy.Clone();
        }

        return clonedGhost;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is not Ghost other)
            return false;

        return Color == other.Color &&
               ghostNo == other.ghostNo &&
               col == other.col &&
               row == other.row;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Color, ghostNo, col, row);
    }
}