using Server.GameWorld;

namespace Server.Classes.Services.Memento;

public class MapMemento : IMapMemento
{
    private GameMap _map;

    public MapMemento(GameMap map)
    {
        _map = (GameMap)map.Clone();
    }

    public void GetMap(MapOriginator originator)
    {
        originator.SetMap(this._map);
    }
}