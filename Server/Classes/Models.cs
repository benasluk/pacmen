using Microsoft.EntityFrameworkCore;
using SharedLibs;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public class GameDbContext : DbContext
{
    public DbSet<InputLog> InputLogs{ get; set; }
    public DbSet<MapLog> MapLogs { get; set; }
    public DbSet<TextLog> TextLogs { get; set; }

    public string DbPath { get; }

    public GameDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "game.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class MapLog
{
    public int Id { get; set; }
    public DateTime LoggedAt { get; set; }

    public string Map { get; set; }
}

public class InputLog
{
    public int Id { get; set; }

    public DateTime LoggedAt { get; set; }
    public Direction Direction { get; set; }
}
public class TextLog
{
    public int Id { get; set; }

    public DateTime LoggedAt { get; set; }
    public string Text { get; set;}

}