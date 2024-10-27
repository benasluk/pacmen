using Server.Classes.Services.Strategy;
using SharedLibs;

namespace Server.Classes.Services.Builder;

public class OrangeGhostBuilder : GhostBuilder
{
    public override GhostBuilder SetColor()
    {
        _ghost.Color = "Orange";
        return this;
    }

    public override GhostBuilder SetMovementStrategy()
    {
        _ghost.SetMovementStrategy(new BFSStrategy());
        return this;
    }

    public override GhostBuilder SetStartingCoordinates()
    {
        _ghost.SetCoordinates(12, 17);
        return this;
    }

    public override GhostBuilder SetGhostNumber()
    {
        _ghost.ghostNo = TileStatus.Ghost1;
        return this;
    }
}