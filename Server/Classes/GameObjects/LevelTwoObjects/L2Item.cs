using Server.Classes.GameLogic;
using Server.Classes.Services;

namespace Server.Classes.GameObjects.LevelTwoObjects;

// #NEW
public class L2Item : Item
{
    public L2Item(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
        Icon = "Orange";
    }
}