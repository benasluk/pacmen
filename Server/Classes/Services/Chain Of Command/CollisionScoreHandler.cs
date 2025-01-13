using Server.Classes.Services.Bridge;
using Server.Classes.Services.Factory;
using Server.Classes.Services.Flyweight;

namespace Server.Classes.Services.Chain_Of_Command
{
    public class CollisionScoreHandler : CollisionHandler
    {
        public override void HandleCollision(CollisionEvent collision)
        {
            if (Pellet.Collides(collision.CollidedTile))
            {
                var totalScore = PlayerScoreSingleton.getInstance().GetScore().Sum();
                var currentScore = PlayerScoreSingleton.getInstance().GetScore()[(int)collision.Player.pacmanNo - 5];
                var time = collision.GameLoop.gameTimer;

                var pelletScore = Pellet.GetPelletScore(collision.CollidedTile);
                var calculatedScore = ServiceLocator.GetService<ScoreCalculator>().CalculateScore(totalScore, currentScore, time);
                var newScore = pelletScore + calculatedScore;

                //Console.WriteLine($"Score updated: {newScore} points added.");
                collision.Player.score += newScore; // Update the player's local score

                PlayerScoreSingleton.getInstance().SetScore((int)collision.Player.pacmanNo - 5, collision.Player.score);
            }

            _nextHandler?.HandleCollision(collision);
        }
    }
}
