using Server.Classes.GameObjects;

namespace Server.Classes.Services.Visitor;

public class FileLoggingVisitor : LoggingVisitor
{
    private readonly string _logFilePath;

    public FileLoggingVisitor(string logFilePath)
    {
        _logFilePath = logFilePath;

        if (File.Exists(_logFilePath))
        {
            File.Delete(_logFilePath);
        }
        using (File.Create(_logFilePath)) { }
        
    }

    public void LogPlayer(Player pacman)
    {
        (int col, int row) = pacman.GetCurrentLocation();
        WriteToFile($"[File] Player {pacman.pacmanNo} location: {row}, {col}");
    }

    public void LogGhost(Ghost ghost)
    {
        (int col, int row) = ghost.GetCurrentLocation();
        WriteToFile($"[File] Ghost {ghost.ghostNo} location: {row}, {col}");
    }

    private void WriteToFile(string message)
    {
        try
        {
            File.AppendAllText(_logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Unable to write to log file: {ex.Message}");
        }
    }
}