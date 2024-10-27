using Xunit;
using Server.GameWorld;
using SharedLibs;

public class GameMapTests
{
    // Mock class to test GameMap, since GameMap is abstract
    private class MockGameMap : GameMap
    {
        public MockGameMap(int rowC, int colC) : base(rowC, colC) { }

        protected override void InitializeMap()
        {
            // Initialize with empty tiles for testing purposes
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _tileStatus[i, j] = TileStatus.Empty;
                }
            }
        }
    }

    [Fact]
    public void GameMap_InitializesWithCorrectSize()
    {
        var map = new MockGameMap(10, 15);
        var size = map.GetMapSize();
        Assert.Equal(10, size.rows);
        Assert.Equal(15, size.col);
    }

    [Fact]
    public void GetTileStatus_OutOfBounds_ThrowsException()
    {
        var map = new MockGameMap(5, 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => map.GetTileStatus(6, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => map.GetTileStatus(0, -1));
    }

    [Fact]
    public void UpdateTile_ChangesTileStatusCorrectly()
    {
        var map = new MockGameMap(5, 5);
        map.UpdateTile(2, 2, TileStatus.Wall);
        var status = map.GetTileStatus(2, 2);
        Assert.Equal(TileStatus.Wall, status);
    }

    [Fact]
    public void RestartMap_ResetsTileStatuses()
    {
        var map = new MockGameMap(3, 3);
        map.UpdateTile(1, 1, TileStatus.Wall);
        map.RestartMap();
        Assert.Equal(TileStatus.Empty, map.GetTileStatus(1, 1));
    }
}
