﻿using System.Runtime.CompilerServices;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.GameObjects
{
    public class Player : GameObject
    {
        public Player(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
        {
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
                x = projectedX;
                y = projectedY;
            }
        }

        public override void UpdateDirection(Direction newDirection)
        {
            this.direction = newDirection;
        }

        private static bool ValidMove(GameMap map, int x, int y)
        {
            var tile = map.GetTileStatus(y, x);
            return !tile.Equals(TileStatus.Wall) && !tile.Equals(TileStatus.Empty);
        }
    }
}
