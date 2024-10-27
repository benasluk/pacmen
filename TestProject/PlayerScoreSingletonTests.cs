using Xunit;
using Server.Classes.Services.Factory;

public class PlayerScoreSingletonTests
{
    private readonly PlayerScoreSingleton _scoreService;

    public PlayerScoreSingletonTests()
    {
        _scoreService = PlayerScoreSingleton.getInstance();
    }

    [Fact]
    public void GetInstance_ReturnsSameInstance()
    {
        var instance1 = PlayerScoreSingleton.getInstance();
        var instance2 = PlayerScoreSingleton.getInstance();
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void SetScore_UpdatesScoreCorrectly()
    {
        _scoreService.SetScore(0, 10);
        Assert.Equal(10, _scoreService.GetScore()[0]);
    }

    [Fact]
    public void SetScore_ThrowsExceptionForInvalidPlayerIndex()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _scoreService.SetScore(-1, 10));
        Assert.Throws<ArgumentOutOfRangeException>(() => _scoreService.SetScore(4, 10));
    }

    [Fact]
    public void ResetScore_SetsAllScoresToZero()
    {
        _scoreService.SetScore(0, 20);
        _scoreService.ResetScore();
        Assert.All(_scoreService.GetScore(), score => Assert.Equal(0, score));
    }

    [Fact]
    public void ResetAfterLevelChange_ResetsScores()
    {
        _scoreService.SetScore(0, 15);
        _scoreService.ResetAfterLevelChange();
        Assert.All(_scoreService.GetScore(), score => Assert.Equal(0, score));
    }
}
