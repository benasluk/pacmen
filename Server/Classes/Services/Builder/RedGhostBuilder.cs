using Server.Classes.Services.Strategy;
using SharedLibs;

namespace Server.Classes.Services.Builder;

public class RedGhostBuilder : GhostBuilder
{
    public override GhostBuilder SetColor()
    {
        _ghost.Color = "Red";
        return this;
    }

    public override GhostBuilder SetMovementStrategy()
    {
        _ghost.SetMovementStrategy(new WallFollowingStrategy());
        return this;
    }

    public override GhostBuilder SetStartingCoordinates()
    {
        _ghost.SetCoordinates(12, 17);
        return this;
    }

    public override GhostBuilder SetGhostNumber()
    {
        _ghost.ghostNo = TileStatus.Ghost2;
        return this;
    }
}