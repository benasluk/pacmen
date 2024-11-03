namespace Server.Classes.Services.Bridge
{
    public class TimeBasedCalculator : ScoreCalculator
    {
        private readonly ICalculationMethod _calculationMethod;
        private readonly Random _random = new Random();

        public TimeBasedCalculator(ICalculationMethod calculationMethod)
        {
            _calculationMethod = calculationMethod;
        }

        public override int CalculateScore(int totalScore, int currPlayerScore, int timerValue)
        {
            int extraScore = _calculationMethod.CalculateExtraScore(currPlayerScore, totalScore);

            if (timerValue <= 10 && _random.NextDouble() < 0.3) // 30% chance in first 10 seconds
            {
                extraScore += 5;
            }

            return extraScore;
        }
    }
}
