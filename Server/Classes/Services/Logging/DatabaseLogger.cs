using SharedLibs;

namespace Server.Classes.Services.Logging
{
    public class DatabaseLogger
    {

        public DatabaseLogger()
        {
        }

        public void Log(string message)
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

        public void LogMap(TileStatus[,] tiles)
        {
            using (var context = new GameDbContext())
            {

                var mapLog = new MapLog
                {
                    LoggedAt = DateTime.UtcNow,
                    Map = Utility.TileMapToString(tiles)
                };

                context.MapLogs.Add(mapLog);
                context.SaveChanges();
            }
        }

        public void LogInput(Direction dir)
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
