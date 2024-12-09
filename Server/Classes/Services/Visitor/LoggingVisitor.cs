using Server.Classes.GameObjects;

namespace Server.Classes.Services.Visitor;

public interface LoggingVisitor
{
    void LogPlayer(Player pacman);
    void LogGhost(Ghost ghost);
}