namespace Server.Classes.Services.Bridge
{
    public class NormalScoreCalculator : ScoreCalculator
    {
        private readonly ICalculationMethod _calculationMethod;
        public NormalScoreCalculator(ICalculationMethod calculationMethod)
        {
            _calculationMethod = calculationMethod;
        }
        public override int CalculateScore(int totalScore, int currPlayerScore, int timerValue)
        {
            {
                int extraScore = _calculationMethod.CalculateExtraScore(currPlayerScore, totalScore);
                return extraScore;
            }
        }
    }
}
