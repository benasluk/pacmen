namespace Server.Classes.Services;

public class MovementTimerService
{
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
}