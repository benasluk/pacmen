using Server.Classes.GameObjects;

namespace Server.Classes.Services.Builder;

public abstract class GhostBuilder
{
    protected Ghost _ghost;
    public Ghost GetGhost() => _ghost;
    public abstract GhostBuilder SetColor();
    public abstract GhostBuilder SetMovementStrategy();
    public abstract GhostBuilder SetStartingCoordinates();
    public abstract GhostBuilder SetGhostNumber();

    public GhostBuilder StartNew(Ghost ghost)
    {
        _ghost = ghost;
        return this;
    }
}