namespace Server.Classes.Services.Bridge
{
    public abstract class ScoreCalculator
    {
        public ScoreCalculator() { }
        public abstract int CalculateScore(int totalScore, int currPlayerScore, int timerValue);
    }
}
