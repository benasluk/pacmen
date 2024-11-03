using SharedLibs;

namespace Server.Classes.Services.Logging
{
    public class DatabaseLoggerToWriterAdapter
    {
        private readonly DatabaseLogger _databaseLogger;

        public DatabaseLoggerToWriterAdapter(DatabaseLogger databaseLogger)
        {
            _databaseLogger = databaseLogger;
        }

        public void Log(string message)
        {
            _databaseLogger.Log(message);
        }

        public void LogMap(TileStatus[,] tiles)
        {
            _databaseLogger.LogMap(ConvertTileStatusToString(tiles));
        }

        public void LogInput(Direction dir)
        {
            _databaseLogger.LogInput(dir);
        }
        public string ConvertTileStatusToString(TileStatus[,] tileMap)
        {
            return Utility.TileMapToString(tileMap);
        }

    }
}
