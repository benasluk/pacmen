namespace Server.Classes.Services.Memento;

public class MapCaretaker
{
    private readonly Stack<IMapMemento> _mementoes = new Stack<IMapMemento>();

    public void SaveState(IMapMemento memento)
    {
        _mementoes.Push(memento);
    }

    public IMapMemento? RestoreState()
    {
        return _mementoes.Count > 0 ? _mementoes.Pop() : null;
    }
}