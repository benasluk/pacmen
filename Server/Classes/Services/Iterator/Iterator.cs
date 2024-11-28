namespace Server.Classes.Services.Iterator;

public interface Iterator<T>
{
    bool HasNext();
    T Next();
    void RemoveCurrent();
}