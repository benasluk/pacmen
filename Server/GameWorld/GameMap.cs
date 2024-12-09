using Microsoft.AspNetCore.Cors.Infrastructure;
using Server.Classes.Services.Decorator;
using SharedLibs;

namespace Server.GameWorld
{
    public abstract class GameMap
    {
        protected TileStatus[,] _tileStatus;
        protected int rows;
        protected int cols;
        protected MapDecorator decorator;
        public GameMap(int rowC, int colC)
        {
            rows= rowC;
            cols= colC;
            _tileStatus = new TileStatus[rows, cols];
            decorator = new MapDecorator();
        }

        protected abstract void InitializeMap();

        // Method for getting tile status
        public TileStatus GetTileStatus(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= cols)
                throw new ArgumentOutOfRangeException("Row or column is out of bounds.");

            return _tileStatus[row, col];
        }

        public (int rows, int col) GetMapSize() => (rows, cols);
        
        public Positions GetAllTiles() 
        { 
            var toReturn = new Positions(_tileStatus);
            toReturn.Addons = decorator.GetAllAddons();
            return toReturn;
        }
        public bool IsFinished()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 1; j < cols; j++)
                {
                    if (_tileStatus[i, j] == TileStatus.PelletLarge || _tileStatus[i, j] == TileStatus.PelletSmall || _tileStatus[i, j] == TileStatus.Pellet) return false;
                }
            }
            return true;
        }
        public void UpdateAddon(Addon toUpdate)
        {
            decorator.UpdateAddon(toUpdate);
        }
        public void RemoveAddon(Addon toRemove)
        {
            decorator.Remove(toRemove);
        }
        public TileStatus UpdateTile(int row, int col, TileStatus status)
        {
            TileStatus overwrittenTile = _tileStatus[row, col];
            _tileStatus[row, col] = status;
            return overwrittenTile;
        }
        public void RestartMap()
        {
            InitializeMap();
        }
        private void SetupForDemo()
        {
            //Pirmas kiekvieno entry skaicius - row, einantys is paskos - columns
            List<List<int>> toReplace = [[8, 13, 14], [12, 6, 12, 15, 21], [22, 6, 9, 18, 21], [26, 13, 14], [32, 13, 14]];
            for (int i = 0; i < toReplace.Count; i++)
            {
                for (int j = 1; j < toReplace[i].Count; j++)
                {
                    _tileStatus[toReplace[i][0], toReplace[i][j]] = TileStatus.Wall;
                }
            }
        }
    }
}
