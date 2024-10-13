using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.GameWorld;

namespace Server.Classes.Services.Factory;

// #NEW
public abstract class AbstractLevelFactory
{
    public abstract GameMap CreateMap();
    public abstract Player CreatePacman(GameLoop gameLoop, GameService gameService);
    public abstract List<Item> CreateItems(GameLoop gameLoop, GameService gameService);
    public abstract string GetItemIcon();


}