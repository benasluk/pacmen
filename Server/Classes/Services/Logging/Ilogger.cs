using SharedLibs;

namespace Server.Classes.Services.Logging
{
    public interface Ilogger
    {
        void Log(string message);
        void LogMap(TileStatus[,] tiles);
        void LogInput(Direction dir);
    }
}
