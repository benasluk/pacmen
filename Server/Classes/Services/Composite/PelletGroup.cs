using SharedLibs;

namespace Server.Classes.Services.Composite
{
    public class PelletGroup : PelletComponent
    {
        private List<PelletComponent> _children = new List<PelletComponent>();

        public override int GetPelletScore()
        {
            int totalScore = 0;
            foreach (var child in _children)
            {
                totalScore += child.GetPelletScore();
            }
            return totalScore;
        }

        public override bool Collides(TileStatus pellet)
        {
            foreach (var child in _children)
            {
                if (child.Collides(pellet))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Add(PelletComponent component)
        {
            _children.Add(component);
        }

        public override void Remove(PelletComponent component)
        {
            _children.Remove(component);
        }
    }
}