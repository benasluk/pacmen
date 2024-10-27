namespace Server.Classes.Services;

public class MovementTimerServiceSingleton 
{
    private MovementTimerServiceSingleton()
    {
       
    }
    private static class SingletonHolder
    {
        public static readonly MovementTimerServiceSingleton instance = new MovementTimerServiceSingleton();
    }

    public static MovementTimerServiceSingleton getInstance()
    {
        return SingletonHolder.instance;
    }
    
    private int _pacmanElapsedTime = 0;
    private int _ghostsElapsedTime = 0;
    private readonly int _pacmanMovementInterval = 500;
    private readonly int _enemyMovementInterval = 500;

    public void UpdateElapsedTime(int period)
    {
        _pacmanElapsedTime += period;
        _ghostsElapsedTime += period;
    }

    public bool PacmanCanMove()
    {
        if (_pacmanElapsedTime < _pacmanMovementInterval) return false;
        _pacmanElapsedTime = 0;
        return true;
    }
    public bool EnemyCanMove()
    {
        if (_ghostsElapsedTime < _enemyMovementInterval) return false;
        _ghostsElapsedTime = 0;
        return true;
    }

    public void ResetAfterLevelChange()
    {
        _pacmanElapsedTime = 0;
        _ghostsElapsedTime = 0;
    }
}