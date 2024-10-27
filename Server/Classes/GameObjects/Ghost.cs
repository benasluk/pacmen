using Server.Classes.GameLogic;
using Server.Classes.Services;
using SharedLibs;

namespace Server.Classes.GameObjects;

public class Ghost : GameObject, ICloneable
{
    public string Color;
    public TileStatus ghostNo = TileStatus.Empty;
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
        
        gameMap.UpdateTile(row, col, TileStatus.Empty);

        col = projectedX;
        row = projectedY;
        
        gameMap.UpdateTile(row, col, ghostNo);
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