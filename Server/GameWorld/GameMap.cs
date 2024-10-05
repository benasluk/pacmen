using SharedLibs;

namespace Server.GameWorld
{
    public abstract class GameMap
    {
        protected TileStatus[,] _tileStatus;
        protected int rows;
        protected int cols;
        public GameMap(int rowC, int colC)
        {
            rows= rowC;
            cols= colC;
            _tileStatus = new TileStatus[rows, cols];
        }

        // #NEW
        protected abstract void InitializeMap();

        // Method for getting tile status
        public TileStatus GetTileStatus(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= cols)
                throw new ArgumentOutOfRangeException("Row or column is out of bounds.");

            return _tileStatus[row, col];
        }
        public Positions GetAllTiles() { return new Positions(_tileStatus); }
        public void UpdateTile(int row, int col, TileStatus status)
        {
            _tileStatus[row, col] = status;
        }
    }
}
