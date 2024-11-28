namespace Server.Classes.Services.Iterator;

public class ListIterator<T> : Iterator<T>
{
    private readonly List<T> _list;
    private int _position = 0;

    public ListIterator(List<T> list)
    {
        _list = list;
    }

    public bool HasNext()
    {
        return _position < _list.Count;
    }

    public T Next()
    {
        if (!HasNext())
            throw new ArgumentOutOfRangeException();

        return _list[_position++];
    }

    public void RemoveCurrent()
    {
        if(!HasNext())
            throw new ArgumentOutOfRangeException();

        _list.RemoveAt(_position);
    }
}