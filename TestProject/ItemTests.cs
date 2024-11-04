using Xunit;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.GameObjects;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class ItemTests
{
    private readonly Mock<GameLoop> _gameLoopMock = new Mock<GameLoop>();
    private readonly GameServiceStub _gameServiceStub = new GameServiceStub();
    private readonly ServiceProvider _provider;

    public ItemTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_gameLoopMock);
        services.AddSingleton(_gameServiceStub);
        _provider = services.BuildServiceProvider();
    }

    [Fact]
    public void Item_InitializesCorrectly()
    {
        var item = new Item(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        Assert.NotNull(item);
    }

    [Fact]
    public void Item_IconIsNullInitially()
    {
        var item = new Item(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        Assert.Null(item.Icon);
    }

    [Fact]
    public void HandleMovement_DoesNothing()
    {
        var item = new Item(_provider.GetService<GameLoop>(), _provider.GetService<GameService>());
        item.HandleMovement(); // No exceptions should be thrown
    }
}

public class GameServiceStub : GameService
{

}
