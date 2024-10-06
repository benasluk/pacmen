using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Factory;
using Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://localhost:5026");
builder.Services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
{
    options.PayloadSerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddSingleton<GameLoop>();
builder.Services.AddSingleton<GameHub>();
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<MessageService>();
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
