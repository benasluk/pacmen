using Server.GameWorld;

namespace Server.Classes.Services.Memento;

public class MapOriginator
{
    private GameMap _map;

    public IMapMemento SaveMapState()
    {
        return new MapMemento(_map);
    }

    public void SetMap(GameMap map)
    {
        _map = map;
    }

    public GameMap GetMap()
    {
        return _map;
    }
    public void RestoreMap(IMapMemento memento)
    {
        memento.GetMap(this);
    }
    
    
}