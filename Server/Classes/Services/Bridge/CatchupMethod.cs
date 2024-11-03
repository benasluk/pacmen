namespace Server.Classes.Services.Bridge
{
    public class CatchupMethod : ICalculationMethod
    {
        private readonly Random _random = new Random();

        public int CalculateExtraScore(int currScore, int averageScore)
        {
            double chance = currScore < averageScore ? 0.5 : 0.1; // 50% if behind, 10% if not
            return _random.NextDouble() < chance ? 10 : 0;
        }

    }
}
