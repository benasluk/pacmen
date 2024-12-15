using Server.Classes.Services.Interpreter;

namespace Server.Classes.Services.Command
{
    public interface ICommand
    {
        public bool Execute(string var);
        public bool Undo(string var);
        public bool Interpret(CommandContext context);
        public string Initiator();
    }
}
