using Xunit;
using Server.Classes.Services;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class MovementTimerServiceSingletonTests
{
    private readonly MovementTimerServiceSingleton _service;

    public MovementTimerServiceSingletonTests()
    {
        _service = MovementTimerServiceSingleton.getInstance();
    }

    [Fact]
    public void GetInstance_ReturnsSameInstance()
    {
        var instance1 = MovementTimerServiceSingleton.getInstance();
        var instance2 = MovementTimerServiceSingleton.getInstance();
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void UpdateElapsedTime_IncreasesElapsedTime()
    {
        _service.UpdateElapsedTime(200);
        Assert.False(_service.PacmanCanMove());
        Assert.False(_service.EnemyCanMove());

        _service.UpdateElapsedTime(300);
        Assert.True(_service.PacmanCanMove());
        Assert.True(_service.EnemyCanMove());
    }

    [Fact]
    public void PacmanCanMove_ReturnsTrueWhenIntervalReached()
    {
        _service.UpdateElapsedTime(500);
        Assert.True(_service.PacmanCanMove());
        Assert.False(_service.PacmanCanMove()); // Resets to false after move
    }

    [Fact]
    public void EnemyCanMove_ReturnsTrueWhenIntervalReached()
    {
        _service.UpdateElapsedTime(500);
        Assert.True(_service.EnemyCanMove());
        Assert.False(_service.EnemyCanMove()); // Resets to false after move
    }

    [Fact]
    public void ResetAfterLevelChange_ResetsElapsedTime()
    {
        _service.UpdateElapsedTime(500);
        _service.ResetAfterLevelChange();

        Assert.False(_service.PacmanCanMove());
        Assert.False(_service.EnemyCanMove());
    }
}
