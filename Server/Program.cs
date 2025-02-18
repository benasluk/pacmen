using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Bridge;
using Server.Classes.Services.Factory;
using Server.Classes.Services.Logging;
using Server.Hubs;
using SharedLibs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
if(builder.Configuration["Port"] != "") builder.WebHost.UseUrls("http://0.0.0.0:" + builder.Configuration["Port"]);
builder.Services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
{
    options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
    options.PayloadSerializerSettings.Error = (sender, args) =>
    {
        Console.WriteLine($"JSON Serialization Error: {args.ErrorContext.Error.Message}");
        args.ErrorContext.Handled = true;
    };
});
if(bool.Parse(builder.Configuration["UseDatabase"])) builder.Services.AddDbContext<GameDbContext>(options =>
        options.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/game.db"));
builder.Services.AddSingleton<GameLoop>();
builder.Services.AddSingleton<GameHub>();
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddSingleton<GhostService>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddSingleton<CommandHandler>();
builder.Services.AddSingleton<DatabaseWriter>();
builder.Services.AddSingleton<DatabaseLoggerToWriterAdapter>();
builder.Services.AddSingleton<DatabaseLogger>();
builder.Services.AddTransient<ScoreCalculator,TimeBasedCalculator>();
builder.Services.AddTransient<ICalculationMethod, CatchupMethod>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .SetIsOriginAllowed((host) => true); // Allow any origin
    });
});

var app = builder.Build();
ServiceLocator.Instance = app.Services;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<GameHub>("/Server");
app.MapControllers();
var gameLoop = app.Services.GetRequiredService<GameLoop>();
gameLoop.Start();
app.Run();
