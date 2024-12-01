namespace Server.Classes.Services.Chain_Of_Command
{
    public abstract class CollisionHandler
    {
        protected CollisionHandler? _nextHandler;
        public void SetNext(CollisionHandler nextHandler) => _nextHandler = nextHandler;
        public abstract void HandleCollision(CollisionEvent collision);
    }
}
