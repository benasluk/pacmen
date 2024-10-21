namespace Server.Classes.Services.Command
{
    public class PauseCommand : ICommand
    {
        private GameService _gameService;
        public PauseCommand(GameService gameService)
        {
            _gameService = gameService;
        }
        public bool Execute(string playerId)
        {
            return _gameService.Pause(playerId);
        }

        public bool Undo(string playerId)
        {
            return _gameService.Unpause(playerId);
        }
        public string Initiator()
        {
            return _gameService.PausedBy();
        }
    }
}
