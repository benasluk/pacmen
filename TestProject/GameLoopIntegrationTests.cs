using Xunit;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Server.Hubs;

public class GameLoopIntegrationTests
{
    private readonly GameLoop _gameLoop;
    private readonly GameService _gameService;
    private readonly MessageService _messageService;

    public GameLoopIntegrationTests()
    {
        // Initialize services required for GameLoop
        _gameService = new GameService();
        _messageService = new MessageService();

        // Mock the HubContext since it's not the focus of this test
        var hubContextMock = new Mock<IHubContext<GameHub>>();

        // Initialize GameLoop with the services
        _gameLoop = new GameLoop(_gameService, _messageService, hubContextMock.Object);
    }

    [Fact]
    public void GameLoop_StartAndRestart_CorrectlyResetsLevelAndItems()
    {
        // Arrange
        _gameLoop.Start();

        // Act - Change the level in MessageService and restart the loop
        int initialLevel = 1;
        _messageService.StoreLevelChange(initialLevel);
        _gameLoop.RestartLoop();

        // Assert
        Assert.True(_messageService.IsLevelChange());  // Verify level change was stored
        Assert.Equal(initialLevel, _messageService.GetLevel()); // Verify stored level matches the initial level
        Assert.True(_gameLoop.levelRestarted);  // Verify that GameLoop restarted
    }

    [Fact]
    public void GameLoop_PauseAndUnpause_UpdatesGameServiceState()
    {
        // Arrange
        _gameLoop.Start();
        string playerId = "player1";

        // Act - Pause the game and verify
        bool isPaused = _gameService.Pause(playerId);

        // Assert - Verify pause state
        Assert.True(isPaused);
        Assert.True(_gameService.paused);
        Assert.Equal(playerId, _gameService.PausedBy());

        // Act - Unpause the game with the same player
        bool isUnpaused = _gameService.Unpause(playerId);

        // Assert - Verify unpause state
        Assert.True(isUnpaused);
        Assert.False(_gameService.paused);
        Assert.Null(_gameService.PausedBy());
    }
}
