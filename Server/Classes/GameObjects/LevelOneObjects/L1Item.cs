using Server.Classes.GameLogic;
using Server.Classes.Services;

namespace Server.Classes.GameObjects.LevelOneObjects;

// #NEW
public class L1Item : Item
{
    public L1Item(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
        Icon = "Apple";
    }
}