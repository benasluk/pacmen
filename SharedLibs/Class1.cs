using System;
using System.Collections.Generic;

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
    public enum WallColor
    {
        Default,
        Yellow,
        Red,
        Green
    }
    public enum PelletColor
    {
        Default,
        Green,
        Red,
        White
    }
    public enum PelletShape
    {
        Default,
        Hexagon,
        Square,
        Triangle
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
        Pacman3,
        Pacman4,
        Ghost1,
        Ghost2,
        Ghost3,
        Ghost4
    }
    public enum CommandType
    {
        Move,
        Pause
    }
    public enum CommandAction
    {
        Execute,
        Undo
    }
    public class MapWall : Addon
    {
        public WallColor color;
        public override void SetValueFromString(string value)
        {
            if(Enum.TryParse(typeof(WallColor), value, false, out object result)) color = (WallColor)result;
        }
        public override string GetValue()
        {
            return color.ToString();
        }
    }
    public class MapPelletColor : Addon
    {
        public PelletColor color;
        public override void SetValueFromString(string value)
        {
            if (Enum.TryParse(typeof(PelletColor), value, false, out object result)) color = (PelletColor)result;
        }
        public override string GetValue()
        {
            return color.ToString();
        }
    }
    public class MapPelletShape : Addon
    {
        public PelletShape shape;
        public override void SetValueFromString(string value)
        {
            if (Enum.TryParse(typeof(PelletShape), value, false, out object result)) shape = (PelletShape)result;
        }
        public override string GetValue()
        {
            return shape.ToString();
        }
    }
    public abstract class Addon
    {
        public abstract void SetValueFromString(string value);
        public abstract string GetValue(); 
    }
    public class Positions
    {
        public TileStatus[,] Grid { get; set; }
        public List<PositionUpdate> MovedObjects { get; set; } = new List<PositionUpdate>();
        public int secondsElapsed { get; set; } = 0;
        public int[] Scores { get; set; }
        public bool SceneChange = false;
        public List<Addon> Addons { get; set; }
        public string[] PlayerColors { get; set; }
        public string[] ItemIcon { get; set; }

        public Positions(TileStatus[,] grid)
        {
            Grid = grid;
        }

        public void AddMovement(string id, int previousX, int previousY, int newX, int newY, bool isPlayer)
        {
            MovedObjects.Add(new PositionUpdate
            {
                Id = id,
                PreviousX = previousX,
                PreviousY = previousY,
                NewX = newX,
                NewY = newY,
                IsPlayer = isPlayer
            });
        }
    }
    public class PositionUpdate
    {
        public string Id { get; set; }  //
        public int PreviousX { get; set; }
        public int PreviousY { get; set; }
        public int NewX { get; set; }
        public int NewY { get; set; }
        public bool IsPlayer { get; set; }  
    }
    public class HandShake
    {
        public string PlayerName { get; set; } //cia turetum atsiust savo nick, bet uhh galimai reiktu ir return objekto, kuris patvirtintu statusa or something idfk

        public HandShake(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
