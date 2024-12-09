using Server.Classes.Services;
using Server.Classes.Services.Visitor;
using SharedLibs;

namespace Server.Classes.GameObjects;

public abstract class UnityObject
{
    protected int ID;
    protected int col;
    protected int row;
    protected Direction direction = Direction.Up;
    
    public virtual void Destroy()
    {
    }
    
    public abstract void HandleMovement();
    
    public abstract void UpdateDirection(Direction newDirection);

    public abstract (int col, int row) GetCurrentLocation();

    public abstract void Accept(LoggingVisitor visitor);
}