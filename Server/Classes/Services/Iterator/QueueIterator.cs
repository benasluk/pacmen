namespace Server.Classes.Services.Iterator;

public class QueueIterator<T> : Iterator<T>
{
    private readonly Queue<T> _queue;

    public QueueIterator(Queue<T> queue)
    {
        _queue = queue;
    }


    public bool HasNext()
    {
        return _queue.Count > 0;
    }

    public T Next()
    {
        if (!HasNext())
            throw new ArgumentOutOfRangeException();

        return _queue.Dequeue();
    }

    public void RemoveCurrent()
    {
        Next();
    }
}