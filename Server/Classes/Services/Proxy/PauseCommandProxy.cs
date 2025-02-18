﻿using Server.Classes.Services.Command;
using Server.Classes.Services.Interpreter;

namespace Server.Classes.Services.Proxy
{
    public class PauseCommandProxy : ICommand
    {
        private readonly PauseCommand _pauseCommand;
        private readonly string _adminId;

        public PauseCommandProxy(PauseCommand pauseCommand, string adminId)
        {
            _pauseCommand = pauseCommand;
            _adminId = adminId;
        }

        public bool Execute(string playerId)
        {
            // Allow any player to pause the game
            return _pauseCommand.Execute(playerId);
        }

        public bool Undo(string playerId)
        {
            if (_pauseCommand.Initiator() != playerId && playerId != _adminId)
            {
                throw new UnauthorizedAccessException("Only the admin can unpause the game if another player paused it.");
            }

            return _pauseCommand.Undo(playerId);
        }

        public string Initiator()
        {
            return _pauseCommand.Initiator();
        }

        public bool Interpret(CommandContext context)
        {
            if(context.Undo)
            {
                if (context.Sender.Equals(_adminId) || context.Sender.Equals(Initiator())) return _pauseCommand.Interpret(context);
            }
            if (!context.Undo) return _pauseCommand.Interpret(context);
            else return false;
        }
    }
}