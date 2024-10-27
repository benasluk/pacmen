using Xunit;
using Moq;
using Server.GameWorld;
using Server.Classes.Services;

public class GameServiceTests
{
    private readonly GameService _gameService;
    private readonly Mock<GameMap> _gameMapMock;

    public GameServiceTests()
    {
        _gameService = new GameService();
        _gameMapMock = new Mock<GameMap>(36, 28);
    }

    [Fact]
    public void SetGameMap_SetsGameMapCorrectly()
    {
        _gameService.SetGameMap(_gameMapMock.Object);
        Assert.Equal(_gameMapMock.Object, _gameService.GetGameMap());
    }

    [Fact]
    public void Pause_SetsPausedStateAndPauser()
    {
        string playerId = "player1";
        _gameService.Pause(playerId);
        Assert.True(_gameService.paused);
        Assert.Equal(playerId, _gameService.PausedBy());
    }

    [Fact]
    public void Unpause_SetsPausedStateToFalse_WhenCalledByPauser()
    {
        string playerId = "player1";
        _gameService.Pause(playerId);
        Assert.True(_gameService.Unpause(playerId));
        Assert.False(_gameService.paused);
        Assert.Null(_gameService.PausedBy());
    }

    [Fact]
    public void Unpause_DoesNotChangePausedState_WhenCalledByDifferentPlayer()
    {
        string playerId = "player1";
        _gameService.Pause(playerId);
        Assert.False(_gameService.Unpause("player2"));
        Assert.True(_gameService.paused);
        Assert.Equal(playerId, _gameService.PausedBy());
    }
}
