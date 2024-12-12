using Server.GameWorld;

namespace Server.Classes.Services.Memento;

public class MapMemento : IMapMemento
{
    private GameMap _map;

    public MapMemento(GameMap map)
    {
        _map = map;
    }

    public void GetMap(MapOriginator originator)
    {
        originator.SetMap(this._map);
    }
}