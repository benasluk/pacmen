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
        Pacman
    }
    public class Positions
    {
        public TileStatus[,] Grid { get; set; }
    }
}
