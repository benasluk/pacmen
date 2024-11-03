using SharedLibs;

namespace Server.Classes
{
    public class Utility
    {
        public static string TileMapToString(TileStatus[,] tiles)
        {
            string result = string.Empty; 
            foreach (TileStatus tileRow in tiles)
            {
                result += tileRow.ToString();
            }
            return result;
        }
    }
}
