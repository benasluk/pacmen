using Newtonsoft.Json;
using Server.Classes.GameLogic;
using Server.Classes.Services;
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
//Atkomentuot, kai rodom demo
//builder.WebHost.UseUrls("http://192.168.0.113:7255");
builder.Services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
{
    options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.PayloadSerializerSettings.Converters.Add(new AddonConverter());
});
builder.Services.AddSingleton<GameLoop>();
builder.Services.AddSingleton<GameHub>();
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddSingleton<GhostService>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddSingleton<CommandHandler>();
builder.Services.AddSingleton<DatabaseWriter>();
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
using var db = new GameDbContext();
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
Console.WriteLine(db.DbPath);
MapLog test = new MapLog();
test.LoggedAt = DateTime.Now;
var gameLoop = app.Services.GetRequiredService<GameLoop>();
gameLoop.Start();
app.Run();
