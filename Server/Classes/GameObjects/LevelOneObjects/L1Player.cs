﻿using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Bridge;

namespace Server.Classes.GameObjects.LevelOneObjects;

public class L1Player : Player
{
    public string BackgroundColor;
    
    public L1Player(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
    {
        BackgroundColor = "Pink";
    }
    
}