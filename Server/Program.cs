using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Bridge;
using Server.Classes.Services.Factory;
using Server.Classes.Services.Logging;
using Server.Hubs;
using SharedLibs;

using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Build.Framework;
using Microsoft.Build.Locator;

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
    options.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
    options.PayloadSerializerSettings.Error = (sender, args) =>
    {
        Console.WriteLine($"JSON Serialization Error: {args.ErrorContext.Error.Message}");
        args.ErrorContext.Handled = true;
    };
});
//builder.Services.AddDbContext<GameDbContext>(options =>
//        options.UseSqlite($"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/game.db"));
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

await Test();

var gameLoop = app.Services.GetRequiredService<GameLoop>();
gameLoop.Start();
app.Run();

async Task Test()
{
    Console.WriteLine("Started analysis");



    var instance = MSBuildLocator.QueryVisualStudioInstances().FirstOrDefault();
    if (instance != null)
    {
        MSBuildLocator.RegisterInstance(instance);
        Console.WriteLine($"Registered MSBuild from: {instance.MSBuildPath}");
    }
    else
    {
        Console.WriteLine("No MSBuild instance found.");
    }

    //MSBuildLocator.RegisterDefaults();

    if (!MSBuildLocator.IsRegistered)
    {
        Console.WriteLine("MSBuildLocator failed to register. Ensure you have the correct MSBuild tools installed.");
        return;
    }

    var workspace = Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.Create();

    workspace.WorkspaceFailed += (sender, e) =>
    {
        Console.WriteLine($"Workspace failed: {e.Diagnostic.Message}");
    };


    var solutionPath = @"C:\Users\bembe\Desktop\KTU\Design patterns\pacmen\Server\Server.sln";
    Solution solution = null;
    try
    {
        solution = await workspace.OpenSolutionAsync(solutionPath);
        Console.WriteLine($"Successfully opened solution: {solution.FilePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to open solution at {solutionPath}. Error: {ex.Message}");
    }

    if (!File.Exists(solutionPath))
    {
        Console.WriteLine($"The specified solution path does not exist: {solutionPath}");
        return;
    }

    foreach (var project in solution.Projects)
    {
        Console.WriteLine($"Analyzing project: {project.Name}");
        foreach (var document in project.Documents)
        {
            var syntaxTree = await document.GetSyntaxTreeAsync();
            if (syntaxTree != null)
            {
                AnalyzeSyntaxTree(syntaxTree);
            }
        }
    }
}

static void AnalyzeSyntaxTree(SyntaxTree syntaxTree)
{
    var root = syntaxTree.GetRoot();

    // Find all literal expressions
    var literals = root.DescendantNodes().OfType<LiteralExpressionSyntax>();

    // Use a HashSet to keep track of lines already reported
    var reportedLines = new HashSet<int>();

    foreach (var literal in literals)
    {
        // Get the line number of the literal
        var lineSpan = literal.GetLocation().GetLineSpan();
        int lineNumber = lineSpan.StartLinePosition.Line;

        // If this line hasn't been reported yet
        if (!reportedLines.Contains(lineNumber))
        {
            // Mark the line as reported
            reportedLines.Add(lineNumber);

            // Get the full line of code
            var lineText = literal.SyntaxTree.GetText().Lines[lineNumber].ToString();

            // Print the line containing the hardcoded value
            Console.WriteLine($"Hardcoded value found on line {lineNumber + 1}: {lineText}");
        }
    }
}
