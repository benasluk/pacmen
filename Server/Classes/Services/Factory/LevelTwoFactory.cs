using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.GameObjects.LevelTwoObjects;
using Server.GameWorld;
using Server.GameWorld.LevelMap;

namespace Server.Classes.Services.Factory;

public class LevelTwoFactory : AbstractLevelFactory
{
    public override GameMap CreateMap()
    {
        return new L2GameMap(36, 28);
    }

    public override Player CreatePacman(GameLoop gameLoop, GameService gameService)
    {
        return new L2Player(gameLoop, gameService);
    }

    public override List<Item> CreateItems(GameLoop gameLoop, GameService gameService)
    {
        List<Item> ItemList = new List<Item>();
        
        for (int i = 0; i < 4; i++)
        {
            var item = new L2Item(gameLoop, gameService);
            ItemList.Add(item);
        }

        return ItemList;
    }
}