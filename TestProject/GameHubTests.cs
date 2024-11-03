using Xunit;
using Moq;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Classes.Services;
using SharedLibs;
using System.Threading.Tasks;
using Server.Classes.GameLogic;
using Microsoft.Extensions.DependencyInjection;

public class GameHubTests
{
    private readonly Mock<MessageService> _messageServiceMock = new Mock<MessageService>();
    private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
    private readonly Mock<GameLoop> _gameLoopMock = new Mock<GameLoop>();

    private readonly ServiceProvider _provider;

    private readonly GameHub _gameHub;

    public GameHubTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_messageServiceMock);
        services.AddSingleton(_gameServiceMock);
        services.AddSingleton(_gameLoopMock);
        _provider = services.BuildServiceProvider();
        _gameHub = new GameHub(_provider.GetService<Mock<MessageService>>().Object, _provider.GetService<Mock<GameService>>().Object, _provider.GetService<GameLoop>());
    }

    [Fact]
    public async Task ReceivedDirection_ValidMovement_CallsStorePlayerInput()
    {
        var movement = new PacmanMovement { PlayerId = "test", Direction = Direction.Up };
        var contextMock = new Mock<HubCallerContext>();
        contextMock.Setup(c => c.ConnectionId).Returns("testConnection");
        _gameHub.Context = contextMock.Object;

        await _gameHub.ReceivedDirection(movement);

        _messageServiceMock.Verify(m => m.StorePlayerInput("testConnection", movement), Times.Once);
    }

}
