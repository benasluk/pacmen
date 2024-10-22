using Server.Classes.GameObjects;

namespace Server.Classes.Services.Builder;

public class GhostDirector
{
    private readonly GhostBuilder _ghostBuilder;

    public GhostDirector(GhostBuilder ghostBuilder)
    {
        _ghostBuilder = ghostBuilder;
    }

    public void Construct(Ghost ghost)
    {
        _ghostBuilder.StartNew(ghost)
            .SetColor()
            .SetMovementStrategy()
            .SetStartingCoordinates()
            .SetGhostNumber();
    }

    public Ghost GetGhost() => _ghostBuilder.GetGhost();
}