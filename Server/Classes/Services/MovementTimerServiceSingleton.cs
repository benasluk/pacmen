using Server.Classes.Services.Observer;

namespace Server.Classes.Services;

public class MovementTimerServiceSingleton : IResetabbleLoop
{
    private MovementTimerServiceSingleton()
    {
        ((IResetabbleLoop)this).SubscriberToLevelChange();

    }
    private static class SingletonHolder
    {
        public static readonly MovementTimerServiceSingleton instance = new MovementTimerServiceSingleton();
    }

    public static MovementTimerServiceSingleton getInstance()
    {
        return SingletonHolder.instance;
    }
    
    private int _elapsedTime = 0;
    private readonly int _pacmanMovementInterval = 500;
    private readonly int _enemyMovementInterval = 500;

    public void UpdateElapsedTime(int period)
    {
        _elapsedTime += period;
    }

    public bool PacmanCanMove()
    {
        if (_elapsedTime < _pacmanMovementInterval) return false;
        _elapsedTime = 0;
        return true;
    }
    public bool EnemyCanMove()
    {
        if (_elapsedTime < _enemyMovementInterval) return false;
        _elapsedTime = 0;
        return true;
    }

    public void ResetAfterLevelChange()
    {
        _elapsedTime = 0;
    }
}