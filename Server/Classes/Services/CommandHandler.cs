using Server.Classes.Services.Command;
using Server.Classes.Services.Interpreter;
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
        public int HandleMessages(int initialState)
        {
            ReadMessages();
            int toReturn = HandleCommands(initialState);
            ClearCommands();
            return toReturn;
        }
        private void ReadMessages()
        {
            commands = _messageService.GetAndClearCommand();
        }
        private int HandleCommands(int initialState)
        {
            int stateToReturn = initialState;
            foreach (var command in commands)
            {
                Console.WriteLine(command.Item1.ToString());
                var isUndo = false;
                if (command.Item2 == CommandAction.Undo) isUndo = true;
                CommandContext context = new CommandContext(command.Item3, isUndo);
                command.Item1.Interpret(context);
/*                if (command.Item2 == CommandAction.Execute) {
                    command.Item1.Execute(command.Item3);
                    stateToReturn = 2;
                    continue;
                }
                if (command.Item2 == CommandAction.Undo)
                {
                    command.Item1.Undo(command.Item3);
                    stateToReturn = 1;
                    continue;
                }*/
            }
            return stateToReturn;
        }
        private void ClearCommands()
        {
            commands.Clear();
        }

    }
}
