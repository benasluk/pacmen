using SharedLibs;

namespace Server.GameWorld
{
    public class GameMap
    {
        TileStatus[,] _tileStatus;
        int rows;
        int cols;
        public GameMap()
        {
            _tileStatus = new TileStatus[rows, cols];
            InitializeMap();
        }
        private void SetBorders()
        {


            Enumerable.Range(0, rows).ToList().ForEach(i =>
            {
                _tileStatus[i, 0] = TileStatus.Wall; // Left border
                _tileStatus[i, cols - 1] = TileStatus.Wall; // Right border
            });

            Enumerable.Range(0, cols).ToList().ForEach(j =>
            {
                _tileStatus[0, j] = TileStatus.Wall; // Top border
                _tileStatus[rows - 1, j] = TileStatus.Wall; // Bottom border
            });
        }
    }
}
