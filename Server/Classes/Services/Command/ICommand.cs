namespace Server.Classes.Services.Command
{
    public interface ICommand
    {
        public bool Execute(string var);
        public bool Undo(string var);
        public string Initiator();
    }
}
