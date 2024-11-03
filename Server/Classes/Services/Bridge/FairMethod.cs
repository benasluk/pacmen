namespace Server.Classes.Services.Bridge
{
    public class FairMethod : ICalculationMethod
    {
        readonly Random _random = new Random();

        public int CalculateExtraScore(int currScore, int averageScore)
        {
            // No bonus in FairCalculationMethod (or minimal chance if desired)
            return _random.NextDouble() < 0.05 ? 5 : 0; // 5% chance for a small bonus
        }
    }

}
