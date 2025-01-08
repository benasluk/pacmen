namespace Server.Classes.Services.Interpreter
{
    public class CommandContext 
    {
        public string Sender;
        public bool Undo;

        public CommandContext(string sender, bool undo)
        {
            Sender = sender;
            Undo = undo;
        }
    }
}
