namespace Server.Classes.Services
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
        public static T GetService<T>()
        {
            return Instance.GetRequiredService<T>();
    }
    }
}
