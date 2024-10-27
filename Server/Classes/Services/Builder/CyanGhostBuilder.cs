using Server.Classes.Services.Strategy;
using SharedLibs;

namespace Server.Classes.Services.Builder;

public class CyanGhostBuilder : GhostBuilder
{
    public override GhostBuilder SetColor()
    {
        _ghost.Color = "Cyan";
        return this;
    }

    public override GhostBuilder SetMovementStrategy()
    {
        _ghost.SetMovementStrategy(new RandomChaseStrategy());
        return this;
    }

    public override GhostBuilder SetStartingCoordinates()
    {
        _ghost.SetCoordinates(14, 17);
        return this;
    }

    public override GhostBuilder SetGhostNumber()
    {
        _ghost.ghostNo = TileStatus.Ghost3;
        return this;
    }
}