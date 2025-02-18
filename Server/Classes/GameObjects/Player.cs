﻿using System.Runtime.CompilerServices;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Classes.Services.Bridge;
using Server.Classes.Services.Chain_Of_Command;
using Server.Classes.Services.Factory;
using Server.Classes.Services.Flyweight;
using Server.Classes.Services.Observer;
using Server.Classes.Services.Visitor;
using Server.GameWorld;
using SharedLibs;

namespace Server.Classes.GameObjects
{
    public class Player : GameObject
    {
        public string color;
        public TileStatus pacmanNo = TileStatus.Empty;
        public int score { get; set; } = 0;
        private readonly ScoreCalculator _scoreCalculatorFactory;
        public Player(GameLoop gameLoop, GameService gameService) : base(gameLoop, gameService)
        {
            _scoreCalculatorFactory = ServiceLocator.GetService<ScoreCalculator>();
            _gameLoop.PacmanMovevement += HandleMovement;
        }
        public override void Destroy()
        {
            _gameLoop.PacmanMovevement -= HandleMovement;
            base.Destroy();
        }
        public int[] GetPos()
        {
            return [row, col];
        }
        public override void HandleMovement()
        {
            var gameMap = GetGameService().GetGameMap();
            int projectedX = col;
            int projectedY = row;

            switch (direction)
            {
                //Reikejo sukeist vietom kur plius kur minus, nes pas mus Y values atvirksciai - i apacia dideja Y

                case Direction.None:
                    break;
                case Direction.Up:
                    projectedY -= 1;
                    break;
                case Direction.Down:
                    projectedY += 1;
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

            if (ValidMove(gameMap, projectedX, projectedY))
            {
                var map = GetGameService().GetGameMap();

                map.UpdateTile(row, col, TileStatus.Empty);
                col = projectedX;
                row = projectedY;
                var tile = map.GetTileStatus(row, col);
                var collisionEvent = new CollisionEvent(this, tile, gameMap, _gameLoop,col,row);
                var collisionVerifier = CreateCollisionHandlerChain();
                collisionVerifier.HandleCollision(collisionEvent);
                map.UpdateTile(row, col, pacmanNo);
            }
        }

        public override void Accept(LoggingVisitor visitor)
        {
            visitor.LogPlayer(this);
        }

        private static bool ValidMove(GameMap map, int x, int y)
        {
            var tile = map.GetTileStatus(y, x);
            return !tile.Equals(TileStatus.Wall);
        }

        public void SetXY(int columnToSet, int rowToSet)
        {
            col = columnToSet;
            row = rowToSet;
        }
        private CollisionHandler CreateCollisionHandlerChain()
        {
            var collisionVerifier = new CollisionVerifier();
            var healthHandler = new CollsionHealthHandler();
            var scoreHandler = new CollisionScoreHandler();
            var deletionHandler = new CollisionDeletionHandler();

            collisionVerifier.SetNext(healthHandler);
            healthHandler.SetNext(scoreHandler);
            scoreHandler.SetNext(deletionHandler);

            return collisionVerifier;
        }
    }
}
