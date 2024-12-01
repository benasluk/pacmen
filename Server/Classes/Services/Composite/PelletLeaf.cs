using Server.Classes.Services.Flyweight;
using SharedLibs;

namespace Server.Classes.Services.Composite
{
    public class PelletLeaf : PelletComponent
    {
        private TileStatus _status;

        public PelletLeaf(TileStatus status)
        {
            _status = status;
        }

        public override int GetPelletScore()
        {
            return Pellet.GetPelletScore(_status);
        }

        public override bool Collides(TileStatus pellet)
        {
            return Pellet.Collides(pellet);
        }

        public override void Add(PelletComponent component)
        {
            throw new NotImplementedException("Cannot add to a leaf component.");
        }

        public override void Remove(PelletComponent component)
        {
            throw new NotImplementedException("Cannot remove from a leaf component.");
        }
    }
}
