using System;

namespace SharedLibs
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    public class PacmanMovement
    {
        public string PlayerId { get; set; }
        public Direction Direction { get; set; }
    }
    public class PacmanPositionUpdate
    {
        public string PlayerId { get; set; }
        public int X;
        public int Y;
    }
    public class PelletCollected
    {
        public string PlayerId { get; set;}
        public int X; 
        public int Y;
    }
    public class PacmanDeafeated
    {
        public string PlayerId { get; set; }
    }
    public enum TileStatus
    {
        Wall,
        Empty,
        Pellet,
        Ghost,
        PelletAndGhost,
        Pacman1,
        Pacman2,
        Pacmman3,
        Pacman4 // holy fuck this is terrible
    }
    public class Positions
    {
        public TileStatus[,] Grid { get; set; }

        public Positions(TileStatus[,] grid)
        {
            Grid = grid;
        }
    }
}
