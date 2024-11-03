using SharedLibs;

namespace Server.Classes.Services.Logging
{
    public class TextFileLogger : Ilogger
    {
        public void Log(string message)
        {
            Console.Out.WriteLineAsync(message);
        }

        public void LogInput(Direction dir)
        {
            Console.Out.WriteLineAsync(dir.ToString());
        }

        public void LogMap(TileStatus[,] tiles)
        {
            Console.Out.WriteLineAsync(Utility.TileMapToString(tiles));
        }   
    }
}
