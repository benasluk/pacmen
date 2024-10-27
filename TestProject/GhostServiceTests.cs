using Xunit;
using Moq;
using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

public class GhostServiceTests
{
    private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
    private readonly Mock<GameLoop> _gameLoopMock = new Mock<GameLoop>();
    private readonly GhostService _ghostService;
    private readonly ServiceProvider _provider;

    public GhostServiceTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_gameLoopMock);
        services.AddSingleton(_gameServiceMock);
        services.AddSingleton(_ghostService);
        _provider = services.BuildServiceProvider();
        _ghostService = new GhostService(_provider.GetService<GameService>());
    }

    [Fact]
    public void AddGhosts_AddsCorrectNumberOfGhosts()
    {
        _ghostService.AddGhosts(_provider.GetService<GameLoop>());
        var field = typeof(GhostService).GetField("_ghosts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var ghosts = (List<Ghost>)field.GetValue(_ghostService);

        Assert.Equal(5, ghosts.Count); // 4 clones + 1 shallow copy
    }

    [Fact]
    public void ResetAfterLevelChange_ResetsGhostsCorrectly()
    {
        _ghostService.AddGhosts(_provider.GetService<GameLoop>());
        _ghostService.ResetAfterLevelChange();

        var field = typeof(GhostService).GetField("_ghosts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var ghosts = (List<Ghost>)field.GetValue(_ghostService);

        Assert.Equal(5, ghosts.Count); // Should reset to original state (4 clones + 1 shallow copy)
    }

    [Fact]
    public void UpdateGhostsLocations_CallsHandleMovementOnAllGhosts()
    {
        _ghostService.AddGhosts(_provider.GetService<GameLoop>());
        var field = typeof(GhostService).GetField("_ghosts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var ghosts = (List<Ghost>)field.GetValue(_ghostService);

        var ghostMock = new Mock<Ghost>(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        ghosts.Clear();
        ghosts.Add(ghostMock.Object);

        _ghostService.UpdateGhostsLocations();

        ghostMock.Verify(g => g.HandleMovement(), Times.Once);
    }
}
