﻿using SharedLibs;

namespace Server.GameWorld
{
    public class GameMap
    {
        TileStatus[,] _tileStatus;
        int rows;
        int cols;
        public GameMap(int rowC, int colC)
        {
            rows= rowC;
            cols= colC;
            _tileStatus = new TileStatus[rows, cols];
            InitializeMap();
        }
        private void InitializeMap()
        {
            int[,] pacmanLayout = new int[,]
            {
                //please fix nezinau koks tiksliai yra pacmano layoutas, turetu sutapt su tuo kas yra for real, cia tiesiog bad
                //0  empty ,1 siena, 2 pelletas
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 1, 2, 2, 1},
                {1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 2, 2, 1},
                {1, 2, 1, 2, 1, 2, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 2, 2, 1},
                {1, 2, 1, 2, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 1, 2, 1, 1, 2, 2, 1},
                {1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 1, 1, 2, 2, 1},
                {1, 2, 1, 2, 1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 1, 2, 1, 1, 2, 2, 1},
                {1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    switch (pacmanLayout[i, j])
                    {
                        case 1:
                            _tileStatus[i, j] = TileStatus.Wall;
                            break;
                        case 0:
                            _tileStatus[i, j] = TileStatus.Empty;
                            break;
                        case 2:
                            _tileStatus[i, j] = TileStatus.Pellet;
                            break;
                    }
                }
            }
        }

        // Method for getting tile status
        public TileStatus GetTileStatus(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= cols)
                throw new ArgumentOutOfRangeException("Row or column is out of bounds.");

            return _tileStatus[row, col];
        }
        public TileStatus[,] GetAllTiles() { return _tileStatus; }
        public void UpdateTile(int row, int col, TileStatus status)
        {
            _tileStatus[row, col] = status;
        }
    }
}
