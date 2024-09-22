using System.Runtime.CompilerServices;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.GameObjects
{
    public class Player : GameObject
    {
        public string color;
        public TileStatus pacmanNo = TileStatus.Empty;
        private int score = 0;
        public Player(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
        {
            _gameLoop.PacmanMovevement += HandleMovement;
        }
        public override void Destroy()
        {
            _gameLoop.PacmanMovevement -= HandleMovement;
            base.Destroy();
        }

        public override void HandleMovement()
        {
            var gameMap = GetGameService().GetGameMap();
            int projectedX = x;
            int projectedY = y;

            switch (direction)
            {
                case Direction.None:
                    break;
                case Direction.Up:
                    projectedY += 1;
                    break;
                case Direction.Down:
                    projectedY -= 1;
                    break;
                case Direction.Left:
                    projectedX -= 1;
                    break;
                case Direction.Right:
                    projectedX += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ValidMove(gameMap, projectedY, projectedY))
            {
                var map = GetGameService().GetGameMap();

                map.UpdateTile(x, y, TileStatus.Empty);
                x = projectedX;
                y = projectedY;
                var tile = map.GetTileStatus(x, y);
                if (tile == TileStatus.Pellet)
                {
                    score++;
                }
                map.UpdateTile(x, y, pacmanNo);
            }
        }

        private static bool ValidMove(GameMap map, int x, int y)
        {
            var tile = map.GetTileStatus(y, x);
            return !tile.Equals(TileStatus.Wall);
        }
    }
}
