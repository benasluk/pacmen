using Server.Classes.Services.Flyweight;
using SharedLibs;
using System.Collections;

namespace Server.Classes.Services.Composite
{
    public abstract class PelletComponent
    {
        public abstract int GetPelletScore();
        public abstract bool Collides(TileStatus pellet);
        public abstract void Add(PelletComponent component);
        public abstract void Remove(PelletComponent component);
    }
}
