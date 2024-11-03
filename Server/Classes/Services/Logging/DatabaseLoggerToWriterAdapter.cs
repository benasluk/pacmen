namespace Server.Classes.Services.Logging
{
    public class DatabaseLoggerToWriterAdapter
    {
        private readonly DatabaseWriter _writer;
        public DatabaseLoggerToWriterAdapter(DatabaseWriter db) {
            _writer = db;
        }
    }
}
