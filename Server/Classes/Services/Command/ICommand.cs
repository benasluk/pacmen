namespace Server.Classes.Services.Command
{
    public interface ICommand
    {
        public void Execute(string var);
        public void Undo(string var);
    }
}
