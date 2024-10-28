﻿using Xunit;
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

    private static IServiceProvider Provider () 
    {
        var services = new ServiceCollection();
        services.AddScoped<Mock<GameLoop>>().AddScoped<Mock<GameService>>();
        _provider = services.BuildServiceProvider();
        _ghost = new TestGhost(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        services.AddScoped<Ghost>();
        return services.BuildServiceProvider();
    }
    private class TestGhost : Ghost
    {
        public TestGhost(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService) { }
        public override void HandleMovement() { }
        public override void Destroy() { }
    }

    public GhostTests()
    {
        /*var services = new ServiceCollection();
        services.AddSingleton(_gameLoopMock);
        services.AddSingleton(_gameServiceMock);
        _provider = services.BuildServiceProvider();
        _ghost = new TestGhost(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        services.AddSingleton(_ghost);
        _provider = services.BuildServiceProvider();*/
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
        var gameMapStub = new StubGameMap(36, 28);
        _gameServiceMock.Setup(s => s.GetGameMap()).Returns(gameMapStub);

        _ghost.SetCoordinates(1, 1);
        _ghost.UpdateDirection(Direction.Right);

        _ghost.HandleMovement();

        Assert.Equal((2, 1), _ghost.GetCurrentLocation());
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
// Stub implementation for GameMap
public class StubGameMap : GameMap
{
    public StubGameMap(int rowC, int colC) : base(rowC, colC)
    {
        // Initialize the map as empty tiles
        InitializeMap();
    }

    protected override void InitializeMap()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                _tileStatus[i, j] = TileStatus.Empty;
            }
        }
    }

    // Additional helper method to set specific tile status for the test
    public void SetTileStatus(int row, int col, TileStatus status)
    {
        _tileStatus[row, col] = status;
    }
}
