using Xunit;
using Moq;
using Server.Classes.GameObjects;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using SharedLibs;
using Server.GameWorld;
using Microsoft.Extensions.DependencyInjection;
using Server;

public class GhostTests
{
    private readonly Mock<GameLoop> _gameLoopMock = new Mock<GameLoop>();
    private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
    private readonly Ghost _ghost;
    private readonly ServiceProvider _provider;

    private class TestGhost : Ghost
    {
        public TestGhost(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService) { }
        public override void HandleMovement() { }
        public override void Destroy() { }
    }

    public GhostTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_gameLoopMock);
        services.AddSingleton(_gameServiceMock);
        _provider = services.BuildServiceProvider();
        _ghost = new TestGhost(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        services.AddSingleton(_ghost);
        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public void Ghost_SubscribeToGhostMovementEvent_OnCreation()
    {
        _gameLoopMock.Raise(g => g.GhostMovevement += null);
        _gameLoopMock.VerifyAdd(g => g.GhostMovevement += It.IsAny<GameLoop.GhostMovevementHandler>(), Times.Once);
    }

    [Fact]
    public void Destroy_UnsubscribesFromGhostMovementEvent()
    {
        _ghost.Destroy();
        _gameLoopMock.VerifyRemove(g => g.GhostMovevement -= It.IsAny<GameLoop.GhostMovevementHandler>(), Times.Once);
    }

    [Fact]
    public void HandleMovement_UpdatesPositionCorrectly()
    {
        var gameMapMock = new Mock<GameMap>(10, 10);
        _gameServiceMock.Setup(s => s.GetGameMap()).Returns(gameMapMock.Object);

        _ghost.SetCoordinates(1, 1);
        _ghost.UpdateDirection(Direction.Right);

        _ghost.HandleMovement();

        Assert.Equal((2, 1), _ghost.GetCurrentLocation());
        gameMapMock.Verify(m => m.UpdateTile(1, 1, TileStatus.Empty), Times.Once);
        gameMapMock.Verify(m => m.UpdateTile(1, 2, _ghost.ghostNo), Times.Once);
    }

    [Fact]
    public void SetCoordinates_UpdatesGhostCoordinates()
    {
        _ghost.SetCoordinates(5, 6);
        Assert.Equal((5, 6), _ghost.GetCurrentLocation());
    }

    [Fact]
    public void Clone_ReturnsShallowCopy()
    {
        _ghost.SetCoordinates(2, 3);
        _ghost.Color = "Red";
        _ghost.ghostNo = TileStatus.Ghost1;

        var clone = (Ghost)_ghost.Clone();

        Assert.NotSame(_ghost, clone);
        Assert.Equal(_ghost.Color, clone.Color);
        Assert.Equal(_ghost.ghostNo, clone.ghostNo);
        Assert.Equal(_ghost.GetCurrentLocation(), clone.GetCurrentLocation());
    }

    [Fact]
    public void DeepCopy_ReturnsDeepCopy()
    {
        _ghost.SetCoordinates(2, 3);
        _ghost.Color = "Blue";
        _ghost.ghostNo = TileStatus.Ghost2;

        var deepCopy = _ghost.DeepCopy();

        Assert.NotSame(_ghost, deepCopy);
        Assert.Equal(_ghost.Color, deepCopy.Color);
        Assert.Equal(_ghost.ghostNo, deepCopy.ghostNo);
        Assert.Equal(_ghost.GetCurrentLocation(), deepCopy.GetCurrentLocation());
    }

    [Fact]
    public void Equals_ReturnsTrueForEqualGhosts()
    {
        _ghost.SetCoordinates(2, 3);
        _ghost.Color = "Yellow";
        _ghost.ghostNo = TileStatus.Ghost3;

        var otherGhost = new Ghost(_provider.GetService<GameLoop>(), _provider.GetService<GameService>())
        {
            Color = "Yellow",
            ghostNo = TileStatus.Ghost3
        };
        otherGhost.SetCoordinates(2, 3);

        Assert.True(_ghost.Equals(otherGhost));
    }
}
