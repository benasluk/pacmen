using Server.Classes.GameLogic;
using Server.Classes.Services;

namespace Server.Classes.GameObjects.LevelTwoObjects;

public class L2Player : Player
{
    public string BackgroundColor;
    
    public L2Player(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
        BackgroundColor = "White";
    }
    
}