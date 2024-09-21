namespace Server.Classes.Services;

public class MovementTimerService
{
    private int _elapsedTime = 0;
    private readonly int _movementInterval = 500;

    public void UpdateElapsedTime(int period)
    {
        _elapsedTime += period;
    }

    public bool CanMove()
    {
        if (_elapsedTime < _movementInterval) return false;
        _elapsedTime = 0;
        return true;
    }
}