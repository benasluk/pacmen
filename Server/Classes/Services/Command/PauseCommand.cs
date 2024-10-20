namespace Server.Classes.Services.Command
{
    public class PauseCommand : ICommand
    {
        private GameService _gameService;
        public PauseCommand(GameService gameService)
        {
            _gameService = gameService;
        }
        public void Execute(string playerId)
        {
            _gameService.Pause(playerId);
        }

        public void Undo(string playerId)
        {
            _gameService.Unpause(playerId);
        }
    }
}
