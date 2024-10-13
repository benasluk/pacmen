namespace Server.Classes.Services
{
    public class ItemSingleton
    {
        private static class SingletonHolder
        {
            public static readonly ItemSingleton instance = new ItemSingleton();
        }

        public static ItemSingleton getInstance()
        {
            return SingletonHolder.instance;
        }
    }
}
