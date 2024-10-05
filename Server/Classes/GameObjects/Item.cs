using Server.Classes.GameLogic;
using Server.Classes.Services;

namespace Server.Classes.GameObjects;

// #NEW
public class Item : GameObject
{
    public Item(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
    }

    public override void HandleMovement()
    {
    }
}