using Xunit;
using Moq;
using Microsoft.AspNetCore.SignalR;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;
using System.Collections.Generic;
using Server.Hubs;
using Microsoft.Extensions.DependencyInjection;

public class GameLoopTests
{
    private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
    private readonly Mock<MessageService> _messageServiceMock = new Mock<MessageService>();
    private readonly Mock<IHubContext<GameHub>> _hubContextMock = new Mock<IHubContext<GameHub>>();
    private readonly ServiceProvider _provider;

    private readonly GameLoop _gameLoop;

    public GameLoopTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_gameServiceMock);
        services.AddSingleton(_messageServiceMock);
        services.AddSingleton(_hubContextMock);
        _provider = services.BuildServiceProvider();
        _gameLoop = new GameLoop(_provider.GetService<Mock<GameService>>().Object, _provider.GetService<Mock<MessageService>>().Object, 
            _provider.GetService<Mock<IHubContext<GameHub>>>().Object);
    }

    [Fact]
    public void Start_SetsUpGameCorrectly()
    {
        _gameLoop.Start();
    }

    [Fact]
    public void RestartLoop_ResetsGameState()
    {
        _messageServiceMock.Setup(m => m.GetLevel()).Returns(1);
        _gameLoop.RestartLoop();
        _messageServiceMock.Verify(m => m.GetLevel(), Times.Once);
    }
}
