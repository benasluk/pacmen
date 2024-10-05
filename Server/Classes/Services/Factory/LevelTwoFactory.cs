using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.GameWorld;

namespace Server.Classes.Services.Factory;

// #NEW
public class LevelTwoFactory : AbstractLevelFactory
{
    public override GameMap CreateMap()
    {
        throw new NotImplementedException();
    }

    public override Player CreatePacman(GameLoop gameLoop, GameService gameService)
    {
        throw new NotImplementedException();
    }

    public override List<Item> CreateItems(GameLoop gameLoop, GameService gameService)
    {
        throw new NotImplementedException();
    }
}