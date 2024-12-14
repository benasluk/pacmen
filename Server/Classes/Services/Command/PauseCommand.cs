using Microsoft.AspNetCore.SignalR;
using Server.Classes.Services.Interpreter;
using Server.Hubs;

namespace Server.Classes.Services.Command
{
    public class PauseCommand : ICommand, IInterpreter
    {
        private GameService _gameService;
        private GameHub _gameHub;
        public PauseCommand(GameService gameService, GameHub gameHub)
        {
            _gameService = gameService;
            _gameHub = gameHub;
        }
        public bool Execute(string playerId)
        {
            bool isPaused = _gameService.Pause(playerId);
            _gameHub.Clients.All.SendAsync("SetPaused", isPaused, playerId);
            return isPaused;
        }

        /// <summary>
        /// Krc, cia bad
        /// </summary>
        /// <param name="playerId">Kas inicijuoja</param>
        /// <returns>TRUE, jei zaidimas liko uzpausintas, FALSE, jei atsipausino</returns>
        public bool Undo(string playerId)
        {
            bool wasUnpaused = _gameService.Unpause(playerId);
            _gameHub.Clients.All.SendAsync("SetPaused", !wasUnpaused, playerId);
            return !wasUnpaused;
        }
        public string Initiator()
        {
            return _gameService.PausedBy();
        }

        public void Interpret(CommandContext context)
        {
            if (context.Undo)
            {
                Undo(context.Sender);
            }
            else
            {
                Execute(context.Sender);
            }
        }
    }
}
