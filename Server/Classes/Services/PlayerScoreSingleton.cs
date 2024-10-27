namespace Server.Classes.Services.Factory
{
    public class PlayerScoreSingleton 
    {
        public PlayerScoreSingleton() 
        {

        }
        private static class SingletonHolder
        {
            public static readonly PlayerScoreSingleton instance = new PlayerScoreSingleton();


        }

        public static PlayerScoreSingleton getInstance()
        {
            return SingletonHolder.instance;
        }
        private int[] score = new int[4];
        public void SetScore(int player, int setScore)
        {
            if (player < 0 || player > 3)
                throw new ArgumentOutOfRangeException();
            else
            {
                score[player] = setScore;
            }
        }
        public int[] GetScore()
        {
            return score;
        }
        public void ResetScore()
        {
            score = new int[4];
        }

        public void ResetAfterLevelChange()
        {
            ResetScore();
        }
    }
}
