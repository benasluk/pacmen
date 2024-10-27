using Xunit;
using Moq;
using Server.Classes.Services;
using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using SharedLibs;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

public class PlayerServiceTests
{
    private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
    private readonly PlayerService _playerService;

    private readonly ServiceProvider _provider;

    public PlayerServiceTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_gameServiceMock);
        _provider = services.BuildServiceProvider();

        _playerService = new PlayerService(_provider.GetService<GameService>());
        services.AddSingleton(_playerService);
        _provider = services.BuildServiceProvider();
        
    }

    [Fact]
    public void GetBackgroundName_ReturnsFirstPlayerColor()
    {
        var playerMock = new Mock<Player>(null, null);
        playerMock.Setup(p => p.color).Returns("Blue");

        var field = typeof(PlayerService).GetField("_players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var players = new Dictionary<string, Player> { { "player1", playerMock.Object } };
        field.SetValue(_playerService, players);

        Assert.Equal("Blue", _playerService.GetBackgroundName());
    }

    [Fact]
    public void GetPlayerById_ReturnsCorrectPlayer()
    {
        var playerMock = new Mock<Player>(null, null);

        var field = typeof(PlayerService).GetField("_players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var players = new Dictionary<string, Player> { { "player1", playerMock.Object } };
        field.SetValue(_playerService, players);

        var player = _playerService.GetPlayerById("player1");
        Assert.Equal(playerMock.Object, player);
    }

    [Fact]
    public void RemovePlayer_RemovesPlayerAndDestroysIt()
    {
        var playerMock = new Mock<Player>(null, null);
        playerMock.Setup(p => p.Destroy());

        var field = typeof(PlayerService).GetField("_players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var players = new Dictionary<string, Player> { { "player1", playerMock.Object } };
        field.SetValue(_playerService, players);

        _playerService.RemovePlayer("player1");
        playerMock.Verify(p => p.Destroy(), Times.Once);
        Assert.Empty((Dictionary<string, Player>)field.GetValue(_playerService));
    }

    [Fact]
    public void UpdatePlayerLocation_UpdatesDirectionAndHandlesMovement()
    {
        var movement = new PacmanMovement { PlayerId = "player1", Direction = Direction.Right };
        var playerMock = new Mock<Player>(null, null);

        var field = typeof(PlayerService).GetField("_players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var players = new Dictionary<string, Player> { { "player1", playerMock.Object } };
        field.SetValue(_playerService, players);

        _playerService.UpdatePlayerLocation(movement);
        playerMock.Verify(p => p.UpdateDirection(Direction.Right), Times.Once);
        playerMock.Verify(p => p.HandleMovement(), Times.Once);
    }

    [Fact]
    public void GetPlayerCount_ReturnsCorrectCount()
    {
        var field = typeof(PlayerService).GetField("_players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var players = new Dictionary<string, Player> { { "player1", new Mock<Player>(null, null).Object } };
        field.SetValue(_playerService, players);

        Assert.Equal(1, _playerService.GetPlayerCount());
    }
}
