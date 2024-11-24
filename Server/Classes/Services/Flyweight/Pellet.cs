using SharedLibs;

namespace Server.Classes.Services.Flyweight
{
    public static class Pellet
    {
        public static int GetPelletScore(TileStatus status)
        {
            return PelletFlyweightFactory.GetPelletFlyweight(status)?.score ?? 0;
        }
        public static bool Collides(TileStatus pellet)
        {
            return pellet == TileStatus.Pellet || pellet == TileStatus.PelletSmall || pellet == TileStatus.PelletLarge;
        }
    }
}
