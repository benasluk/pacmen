using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Strategy;
using SharedLibs;

namespace Server.Classes.GameObjects;

public class Ghost : GameObject
{
    public string Color;
    private MovementStrategy _movementStrategy;
    public Ghost(GameLoop gameLoop, GameService gameService, string color) : base(gameLoop, gameService)
    {
        Color = color;
        _movementStrategy = new BFSStrategy();
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

        col = projectedX;
        row = projectedY;
    }

    public void SetMovementStrategy(MovementStrategy movementStrategy)
    {
        _movementStrategy = movementStrategy;
    }


}