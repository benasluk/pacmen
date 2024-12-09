using SharedLibs;

namespace Server.Classes.Services.Logging
{
    public class DatabaseLogger
    {
        private static readonly object _lock = new object();

        public DatabaseLogger()
        {
        }

        public void Log(string message)
        {
            lock (_lock)
            {
                using (var context = new GameDbContext())
                {
                    var textLog = new TextLog
                    {
                        LoggedAt = DateTime.UtcNow,
                        Text = message
                    };

                    context.TextLogs.Add(textLog);
                    context.SaveChanges();
                }
            }
        }

        public void LogMap(string tiles)
        {
            lock (_lock)
            {
                using (var context = new GameDbContext())
                {
                    var mapLog = new MapLog
                    {
                        LoggedAt = DateTime.UtcNow,
                        Map = tiles
                    };

                    context.MapLogs.Add(mapLog);
                    context.SaveChanges();
                }
            }
        }

        public void LogInput(Direction dir)
        {
            lock (_lock)
            {
                using (var context = new GameDbContext())
                {
                    var inputLog = new InputLog
                    {
                        LoggedAt = DateTime.UtcNow,
                        Direction = dir
                    };

                    context.InputLogs.Add(inputLog);
                    context.SaveChanges();
                }
            }
        }
    }
}