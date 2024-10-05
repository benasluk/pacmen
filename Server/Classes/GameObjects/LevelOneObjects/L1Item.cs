using Server.Classes.GameLogic;
using Server.Classes.Services;

namespace Server.Classes.GameObjects.LevelOneObjects;

// #NEW
public class L1Item : Item
{
    public string Icon;
    public L1Item(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
        Icon = "BluePellet";
    }
}