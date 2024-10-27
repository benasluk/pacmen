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
    private readonly Mock<PlayerService> _playerServiceMock = new Mock<PlayerService>();
    private readonly Mock<MessageService> _messageServiceMock = new Mock<MessageService>();
    private readonly Mock<GhostService> _ghostServiceMock = new Mock<GhostService>();
    private readonly Mock<IHubContext<GameHub>> _hubContextMock = new Mock<IHubContext<GameHub>>();
    private readonly ServiceProvider _provider;

    private readonly GameLoop _gameLoop;

    public GameLoopTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_gameServiceMock);
        services.AddSingleton(_playerServiceMock);
        services.AddSingleton(_messageServiceMock);
        services.AddSingleton(_ghostServiceMock);
        services.AddSingleton(_hubContextMock);
        _provider = services.BuildServiceProvider();
        _gameLoop = new GameLoop(_provider.GetService<GameService>(), _provider.GetService<PlayerService>(), 
            _provider.GetService<MessageService>(), _provider.GetService<IHubContext<GameHub>>(), 
            _provider.GetService<GhostService>());
    }

    [Fact]
    public void Start_SetsUpGameCorrectly()
    {
        _gameLoop.Start();
        _ghostServiceMock.Verify(g => g.AddGhosts(It.IsAny<GameLoop>()), Times.Once);
    }

    [Fact]
    public void RestartLoop_ResetsGameState()
    {
        _messageServiceMock.Setup(m => m.GetLevel()).Returns(1);
        _gameLoop.RestartLoop();
        _messageServiceMock.Verify(m => m.GetLevel(), Times.Once);
    }
}
