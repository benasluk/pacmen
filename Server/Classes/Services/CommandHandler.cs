using SharedLibs;

namespace Server.Classes.Services
{
    public class CommandHandler
    {
        private readonly MessageService _messageService;
        private Stack<(Command.ICommand, CommandAction, string)> commands;//this should really be a class
        public CommandHandler(MessageService messageService) 
        {
            _messageService = messageService;
        }
        public void HandleMessages()
        {
            ReadMessages();
            HandleCommands();
            ClearCommands();
        }
        private void ReadMessages()
        {
            commands = _messageService.GetAndClearCommand();
        }
        private void HandleCommands()
        {
            foreach (var command in commands)
            {
                if (command.Item2 == CommandAction.Execute) {
                    command.Item1.Execute(command.Item3);
                    continue;
                }
                if (command.Item2 == CommandAction.Undo)
                {
                    command.Item1.Undo(command.Item3);
                }
            }
        }
        private void ClearCommands()
        {
            commands.Clear();
        }

    }
}
