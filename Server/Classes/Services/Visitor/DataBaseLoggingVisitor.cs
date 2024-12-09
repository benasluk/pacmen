using Server.Classes.GameObjects;
using Server.Classes.Services.Logging;

namespace Server.Classes.Services.Visitor;

public class DataBaseLoggingVisitor : LoggingVisitor
{
    private DatabaseLogger _databaseLogger;
    
    public DataBaseLoggingVisitor()
    {
        _databaseLogger = new DatabaseLogger();
    }
    
    public void LogPlayer(Player pacman)
    {
        (int col, int row) = pacman.GetCurrentLocation();
        string message = $"[DB] Player {pacman.pacmanNo} location: {row}, {col}";
        _databaseLogger.Log(message);
    }

    public void LogGhost(Ghost ghost)
    {
        (int col, int row) = ghost.GetCurrentLocation();
        string message = $"[File] Ghost {ghost.ghostNo} location: {row}, {col}";
        _databaseLogger.Log(message);
    }
}