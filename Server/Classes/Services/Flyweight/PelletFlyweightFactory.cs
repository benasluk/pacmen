using SharedLibs;

namespace Server.Classes.Services.Flyweight
{
    public static class PelletFlyweightFactory
    {
        private static Dictionary<TileStatus, PelletFlyweight> _pellets = new Dictionary<TileStatus, PelletFlyweight>();
        public static PelletFlyweight GetPelletFlyweight(TileStatus status)
        {
            if (!_pellets.ContainsKey(status))
            {
                switch(status)
                {
                    case TileStatus.Pellet:
                        _pellets[status] = new PelletFlyweight(3);
                        break;
                    case TileStatus.PelletSmall:
                        _pellets[status] = new PelletFlyweight(1);
                        break;
                    case TileStatus.PelletLarge:
                        _pellets[status] = new PelletFlyweight(5);
                        break;
                    default:
                        Console.WriteLine("Wrong tile status for flyweight in factory");
                        return null;
                }
            }
            return _pellets[status];
        }

    }
}
