using Xunit;
using Server;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Moq;
using SharedLibs;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

public class GameObjectTests
{
    private readonly Mock<GameLoop> _gameLoopMock = new Mock<GameLoop>();
    private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
    private readonly ServiceProvider _provider;

    public GameObjectTests() {
        var services = new ServiceCollection();
        services.AddSingleton(_gameLoopMock);
        services.AddSingleton(_gameServiceMock);
        _provider = services.BuildServiceProvider();
    }
    private class TestGameObject : GameObject
    {
        public TestGameObject(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService) { }
        public override void HandleMovement() { }
    }

    [Fact]
    public void UpdateDirection_ChangesDirectionCorrectly()
    {
        var gameObject = new TestGameObject(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        gameObject.UpdateDirection(Direction.Right);
        Assert.Equal(Direction.Right, gameObject.GetDirection());
    }

    [Fact]
    public void GetCurrentLocation_ReturnsCorrectLocation()
    {
        var gameObject = new TestGameObject(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        Assert.Equal((0, 0), gameObject.GetCurrentLocation());
    }
}
