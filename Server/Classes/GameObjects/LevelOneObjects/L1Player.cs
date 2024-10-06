using Server.Classes.GameLogic;
using Server.Classes.Services;

namespace Server.Classes.GameObjects.LevelOneObjects;

// #NEW
public class L1Player : Player
{
    public string BackgroundColor;
    
    public L1Player(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
        BackgroundColor = "Pink";
    }
    
}