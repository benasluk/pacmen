using Server.Classes.Services.Composite;
using SharedLibs;

namespace Server.GameWorld.LevelMap;

public class L1GameMap : GameMap
{
    private PelletGroup allPellets = new PelletGroup();
    private PelletComponent regularPellets = new PelletGroup();
    private PelletComponent bigPellets = new PelletGroup();
    private PelletComponent smallPellets = new PelletGroup();
    public L1GameMap(int rowC, int colC) : base(rowC, colC)
    {
        PelletComponent pellet1 = new PelletLeaf(TileStatus.Pellet);
        PelletComponent pellet2 = new PelletLeaf(TileStatus.PelletSmall);
        PelletComponent pellet3 = new PelletLeaf(TileStatus.PelletLarge);

        InitializeMap();
    }

    protected sealed override void InitializeMap()
    {
        PelletComponent pellet1 = new PelletLeaf(TileStatus.Pellet);
        PelletComponent pellet2 = new PelletLeaf(TileStatus.PelletSmall);
        PelletComponent pellet3 = new PelletLeaf(TileStatus.PelletLarge);

        int[,] pacmanLayout = new int[,]
            {
                //0  empty ,1 siena, 2 pelletas
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 5, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 13, 6, 1},
                {1, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1},
                {1, 2, 1, 0, 0, 1, 2, 1, 0, 0, 0, 1, 2, 1, 1, 2, 1, 0, 0, 0, 1, 2, 1, 0, 0, 1, 2, 1},
                {1, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 2, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 2, 1},
                {1, 2, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1},
                {1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 0, 0, 1, 2, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 2, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 1, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 1, 2, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 2, 1, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 2, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 2, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 2, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 0, 0, 1, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 2, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 1, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 1, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 2, 1, 0, 0, 0, 0, 0},
                {1, 1, 1, 1, 1, 1, 2, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 2, 1, 1, 1, 1, 1, 1},
                {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                {1, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1},
                {1, 2, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 2, 1},
                {1, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 1},
                {1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 1},
                {1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 1},
                {1, 2, 2, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 1},
                {1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 1},
                {1, 7, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 8, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            };

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                switch (pacmanLayout[i, j])
                {
                    case 1:
                        _tileStatus[i, j] = TileStatus.Wall;
                        break;
                    case 0:
                        _tileStatus[i, j] = TileStatus.Empty;
                        break;
                    case 2:
                        allPellets.Add(pellet1);
                        regularPellets.Add(new PelletLeaf(TileStatus.Pellet));
                        _tileStatus[i, j] = TileStatus.Pellet;
                        break;
                    case 3:
                        _tileStatus[i, j] = TileStatus.Ghost;
                        break;
                    case 5:
                        _tileStatus[i, j] = TileStatus.Pacman1;
                        break;
                    case 6:
                        _tileStatus[i, j] = TileStatus.Pacman2;
                        break;
                    case 7:
                        _tileStatus[i, j] = TileStatus.Pacman3;
                        break;
                    case 8:
                        _tileStatus[i, j] = TileStatus.Pacman4;
                        break;
                    case 9:
                        _tileStatus[i, j] = TileStatus.Ghost1;
                        break;
                    case 10:
                        _tileStatus[i, j] = TileStatus.Ghost2;
                        break;
                    case 11:
                        _tileStatus[i, j] = TileStatus.Ghost3;
                        break;
                    case 12:
                        _tileStatus[i, j] = TileStatus.Ghost4;
                        break;
                    case 13:
                        allPellets.Add(pellet2);
                        smallPellets.Add(new PelletLeaf(TileStatus.PelletSmall));
                        _tileStatus[i, j] = TileStatus.PelletSmall;
                            break;
                    case 14:
                        allPellets.Add(pellet3);
                        _tileStatus[i, j] = TileStatus.PelletLarge;
                        bigPellets.Add(new PelletLeaf(TileStatus.PelletLarge));
                        break;
                }
            }
        }

        Console.WriteLine($"After initializing map, the total score of all pellets is {allPellets.GetPelletScore()}");
        Console.WriteLine($"The total score of only small pellets is {smallPellets.GetPelletScore()}");
        Console.WriteLine($"The total score of only regular pellets is {regularPellets.GetPelletScore()}");
        Console.WriteLine($"The total score of only big pellets is {bigPellets.GetPelletScore()}");

        //Remove for development
        SetupForDemo();
    }

    private void SetupForDemo()
    {
        //Pirmas kiekvieno entry skaicius - row, einantys is paskos - columns
        List<List<int>> toReplace = [[8, 13, 14], [12, 6, 12, 15, 21], [22, 6, 9, 18, 21], [26, 13, 14], [32, 13, 14]];
        for (int i = 0; i < toReplace.Count; i++)
        {
            for (int j = 1; j < toReplace[i].Count; j++)
            {
                _tileStatus[toReplace[i][0], toReplace[i][j]] = TileStatus.Wall;
            }
        }
    }
}