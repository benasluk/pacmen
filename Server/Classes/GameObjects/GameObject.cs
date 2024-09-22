﻿using Server.Classes.GameLogic;
using Server.Classes.Services;
using SharedLibs;

namespace Server
{
    public abstract class GameObject //Update constructor logic or something
    {
        protected int ID;
        protected int x;
        protected int y;
        protected Direction direction = Direction.None;
        protected readonly GameLoop _gameLoop;
        protected readonly GameService _gameService;
        
        public GameObject(GameLoop gameLoop, GameService gameService)
        {
            _gameLoop = gameLoop;
            _gameService = gameService;
        }
        public virtual void Destroy()
        {
        }
        public abstract void HandleMovement();

        public void UpdateDirection(Direction newDirection)
        {
            this.direction = newDirection;
        }

        protected GameService GetGameService()
        {
            return _gameService;
        }

        public (int x, int y) GetCurrentLocation()
        {
            return (x, y);
        }
    }
}
