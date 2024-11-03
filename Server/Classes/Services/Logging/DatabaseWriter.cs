using SharedLibs;

namespace Server.Classes.Services.Logging
{
    public class DatabaseWriter : Ilogger
    {
        private readonly DatabaseLoggerToWriterAdapter _adapter;

        public DatabaseWriter(DatabaseLoggerToWriterAdapter adapter)
        {
            _adapter = adapter;
        }

        public void Log(string message)
        {
            _adapter.Log(message);
        }

        public void LogMap(TileStatus[,] tiles)
        {
            _adapter.LogMap(tiles);
        }

        public void LogInput(Direction dir)
        {
            _adapter.LogInput(dir);
        }
    }
}
