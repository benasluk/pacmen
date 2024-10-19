namespace Server.Classes.Services.Observer
{
    public static class ResetEvent
    {
        private static List<IResetabbleLoop> _items = new List<IResetabbleLoop>();
        public static void AddToEvent(IResetabbleLoop item)
        {
            _items.Add(item);
        }
        public static void Unsub(IResetabbleLoop item)
        {
            _items.Remove(item);
        }
        public static void ResetLoop()
        {
            foreach (var item in _items)
            {
                item.ResetAfterLevelChange();
            }
        }
    }
}
