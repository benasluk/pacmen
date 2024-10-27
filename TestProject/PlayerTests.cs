using Xunit;
using Moq;
using Server.Classes.GameObjects;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Factory;
using Server.GameWorld;
using SharedLibs;
using Microsoft.Extensions.DependencyInjection;

public class PlayerTests
{
    private readonly Mock<GameLoop> _gameLoopMock = new Mock<GameLoop>();
    private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
    private readonly Player _player;
    private readonly ServiceProvider _provider;

    public PlayerTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_gameLoopMock);
        services.AddSingleton(_gameServiceMock);
        _provider = services.BuildServiceProvider();

        _player = new Player(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        services.AddSingleton(_player);
        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public void Player_SubscribeToPacmanMovementEvent_OnCreation()
    {
        _gameLoopMock.Raise(g => g.PacmanMovevement += null);
        _gameLoopMock.VerifyAdd(g => g.PacmanMovevement += It.IsAny<GameLoop.PacmanMovevementHandler>(), Times.Once);
    }

    [Fact]
    public void Destroy_UnsubscribesFromPacmanMovementEvent()
    {
        _player.Destroy();
        _gameLoopMock.VerifyRemove(g => g.PacmanMovevement -= It.IsAny<GameLoop.PacmanMovevementHandler>(), Times.Once);
    }

    [Fact]
    public void HandleMovement_ValidMove_UpdatesPositionAndScore()
    {
        var gameMapMock = new Mock<GameMap>(10, 10);
        _gameServiceMock.Setup(s => s.GetGameMap()).Returns(gameMapMock.Object);

        gameMapMock.Setup(m => m.GetTileStatus(It.IsAny<int>(), It.IsAny<int>())).Returns(TileStatus.Pellet);
        _player.SetXY(1, 1);
        _player.UpdateDirection(Direction.Right);

        _player.HandleMovement();

        Assert.Equal((2, 1), _player.GetCurrentLocation());
        gameMapMock.Verify(m => m.UpdateTile(1, 1, TileStatus.Empty), Times.Once);
    }

    [Fact]
    public void HandleMovement_InvalidMove_DoesNotUpdatePosition()
    {
        var gameMapMock = new Mock<GameMap>(10, 10);
        _gameServiceMock.Setup(s => s.GetGameMap()).Returns(gameMapMock.Object);

        gameMapMock.Setup(m => m.GetTileStatus(It.IsAny<int>(), It.IsAny<int>())).Returns(TileStatus.Wall);
        _player.SetXY(1, 1);
        _player.UpdateDirection(Direction.Right);

        _player.HandleMovement();

        Assert.Equal((1, 1), _player.GetCurrentLocation());
        gameMapMock.Verify(m => m.UpdateTile(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<TileStatus>()), Times.Never);
    }

    [Fact]
    public void SetXY_UpdatesPlayerCoordinates()
    {
        _player.SetXY(3, 4);
        Assert.Equal((3, 4), _player.GetCurrentLocation());
    }
}
