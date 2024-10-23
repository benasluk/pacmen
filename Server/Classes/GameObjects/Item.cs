using Server.Classes.GameLogic;
using Server.Classes.Services;

namespace Server.Classes.GameObjects;

public class Item : GameObject
{
    public string Icon;
    public Item(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
    }

    public override void HandleMovement()
    {
    }
}