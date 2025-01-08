using Server.Classes.GameObjects;

namespace Server.Classes.Services.Visitor;

public class ConsoleLoggingVisitor : LoggingVisitor
{
    public void LogPlayer(Player pacman)
    {
        (int col, int row) = pacman.GetCurrentLocation();
        //Console.WriteLine($"[Console] Player {pacman.pacmanNo.ToString()} location: {row}, {col}");
    }

    public void LogGhost(Ghost ghost)
    {
        (int col, int row) = ghost.GetCurrentLocation();
        //Console.WriteLine($"[Console] Ghost {ghost.ghostNo.ToString()} location: {row}, {col}");
    }
}