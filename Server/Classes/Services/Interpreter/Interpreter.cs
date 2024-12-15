namespace Server.Classes.Services.Interpreter
{
    public interface IInterpreter
    {
        public bool Interpret(CommandContext context);
    }
}
