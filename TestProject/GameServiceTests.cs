using Xunit;
using Moq;
using Server.GameWorld;
using Server.Classes.Services;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
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

    [Theory]
    [InlineData("player1")]
    [InlineData("player2")]
    public void Pause_SetsPausedStateAndPauser(string playerId)
    {
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

    [Theory]
    [InlineData("player1", "player2")]
    [InlineData("player1", "player3")]
    public void Unpause_DoesNotChangePausedState_WhenCalledByDifferentPlayer(string pauserId, string callerId)
    {
        _gameService.Pause(pauserId);
        Assert.False(_gameService.Unpause(callerId));
        Assert.True(_gameService.paused);
        Assert.Equal(pauserId, _gameService.PausedBy());
    }
}
