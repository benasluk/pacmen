using Xunit;
using Moq;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;
using System.Collections.Generic;

public class MessageServiceTests
{
    private readonly MessageService _messageService;

    public MessageServiceTests()
    {
        var gameServiceMock = new Mock<GameService>();
        var playerServiceMock = new Mock<PlayerService>(gameServiceMock.Object);
        _messageService = new MessageService(gameServiceMock.Object, playerServiceMock.Object);
    }

    [Fact]
    public void StoreLevelChange_SetsNewLevelCorrectly()
    {
        _messageService.StoreLevelChange(1);
        Assert.True(_messageService.IsLevelChange());
        Assert.Equal(1, _messageService.GetLevel());
    }

    [Fact]
    public void ResetLevel_SetsLevelToMinusOne()
    {
        _messageService.StoreLevelChange(1);
        _messageService.ResetLevel();
        Assert.False(_messageService.IsLevelChange());
        Assert.Equal(-1, _messageService.GetLevel());
    }

    [Fact]
    public void StorePlayerInput_AddsInputCorrectly()
    {
        var movement = new PacmanMovement { PlayerId = "player1", Direction = Direction.Right };
        _messageService.StorePlayerInput("player1", movement);

        var inputs = _messageService.GetPlayerInputs();
        Assert.Single(inputs);
        Assert.Equal(movement, inputs["player1"]);
    }

    [Fact]
    public void GetPlayerInputs_ClearsStoredInputs()
    {
        var movement = new PacmanMovement { PlayerId = "player1", Direction = Direction.Up };
        _messageService.StorePlayerInput("player1", movement);

        var inputs = _messageService.GetPlayerInputs();
        Assert.Empty(_messageService.GetPlayerInputs()); // Should be empty after retrieval
    }

    [Fact]
    public void ResetAfterLevelChange_DecreasesLevelAndClearsInputs()
    {
        _messageService.StoreLevelChange(1);
        _messageService.StorePlayerInput("player1", new PacmanMovement());
        _messageService.ResetAfterLevelChange();

        Assert.Equal(0, _messageService.GetLevel());
        Assert.Empty(_messageService.GetPlayerInputs());
    }
}
