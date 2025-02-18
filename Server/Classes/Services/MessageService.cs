﻿using Server.Classes.Services.Command;
using Server.Classes.Services.Logging;
using Server.Classes.Services.Observer;
using Server.GameWorld;
using SharedLibs;
using System.Windows.Input;

namespace Server.Classes.Services
{
    public class MessageService : IResetabbleLoop
    {
        private readonly Dictionary<string, PacmanMovement> _playerInputs = new Dictionary<string, PacmanMovement>();
        private Stack<(Command.ICommand, CommandAction, string)> commands = new Stack<(Command.ICommand, CommandAction, string)>();
        private GameMap _gameMap = null;
        private object lockObj;
        private object levelChangeLock; //None of the methods using this lock can be accessed if atleast one method using this is locked
        private int newLevel = -1;
        private object commandLock;
        private Ilogger textLogger;
        private Ilogger databaseLogger;
        public MessageService(GameService gameService, PlayerService playerService)
        {
            lockObj = new object();
            levelChangeLock = new object();
            commandLock = new object();
            ((IResetabbleLoop)this).SubscriberToLevelChange();
            textLogger = new TextFileLogger();
            databaseLogger = new DatabaseWriter(new DatabaseLoggerToWriterAdapter(new DatabaseLogger()));

        }
        public void StoreLevelChange(int level)
        {
            lock (levelChangeLock)
            {
                if (newLevel == -1 && level > -1 && level < 2)
                {
                    newLevel = level;
                }
            }
        }
        public void ResetLevel()
        {
            lock (levelChangeLock)
            {
                newLevel = -1;
            }
        }
        public bool IsLevelChange()
        {
            lock (levelChangeLock)
            {
                return newLevel != -1;
            }
        }
        public int GetLevel()
        {
            lock (levelChangeLock)
            {
                return newLevel;
            }
        }
        public void StorePlayerInput(string playerId, PacmanMovement input)
        {
            lock (lockObj)
            {
                /*                databaseLogger.LogInput(input.Direction);
                                textLogger.LogInput(input.Direction);*/
                Console.WriteLine($"Got inpurt from player {playerId} it was {input}");
                _playerInputs[playerId] = input;
            }
        }
        public Dictionary<string, PacmanMovement> GetPlayerInputs()
        {
            Dictionary<string, PacmanMovement> inputs;
            lock (lockObj)
            {
                inputs = new Dictionary<string, PacmanMovement>(_playerInputs);
                _playerInputs.Clear();
            }
            return inputs;
        }
        public GameMap StoreMap(GameMap map)
        {
            _gameMap = map;
            return _gameMap;
        }
        public void StoreCommand(Command.ICommand newCommand, CommandAction action, string connectionId)
        {
            lock (commandLock)
            {
                commands.Push((newCommand, action, connectionId));
            }
        }
        public Stack<(Command.ICommand, CommandAction, string)> GetAndClearCommand()
        {
            Stack<(Command.ICommand, CommandAction, string)> returnCommands = new Stack<(Command.ICommand, CommandAction, string)>(commands);
            commands = new Stack<(Command.ICommand, CommandAction, string)>();
            return returnCommands;
        }
        public void ResetAfterLevelChange()
        {
            newLevel -= 1;
            _playerInputs.Clear();
        }
    }
}
