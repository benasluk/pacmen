namespace Server.Classes.Services.Iterator;

public class LinkedListIterator<T> : Iterator<T>
{
    private LinkedListNode<T>? _node;
    private readonly LinkedList<T> _list;

    public LinkedListIterator(LinkedList<T> list)
    {
        _list = list;
        _node = list.First;
    }

    public bool HasNext()
    {
        return _node is not null;
    }

    public T Next()
    {
        if (!HasNext())
            throw new ArgumentOutOfRangeException();

        T value = _node!.Value;
        _node = _node.Next;
        return value;
    }
    
    public void RemoveCurrent()
    {
        if (_node is null)
            throw new ArgumentOutOfRangeException();

        var nodeToRemove = _node.Previous ?? _list.First;
        if (nodeToRemove != null)
            _list.Remove(nodeToRemove);
    }
}