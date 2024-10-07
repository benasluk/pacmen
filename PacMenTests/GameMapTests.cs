using Server.GameWorld.LevelMap;
using SharedLibs;


namespace PacMenTests
{
    [TestClass]
    public class GameMapTests
    {
        [TestMethod]
        public void GetTileStatus_NegativeNumber_ThrowsException()
        {
            L1GameMap gameMap = new L1GameMap(36, 28);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => gameMap.GetTileStatus(0,-5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => gameMap.GetTileStatus(-5,0));
        }

        [TestMethod]
        public void GetTileStatus_PositiveBigNumber_ThrowsException()
        {
            L1GameMap gameMap = new L1GameMap(36, 28);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => gameMap.GetTileStatus(0, 300));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => gameMap.GetTileStatus(300, 0));
        }

        [TestMethod]
        public void GetTileStatus_PositiveNumber_ReturnsEmpty()
        {
            L1GameMap gameMap = new L1GameMap(36, 28);
            Assert.AreEqual(gameMap.GetTileStatus(0, 0), TileStatus.Empty);
            Assert.AreEqual(gameMap.GetTileStatus(13, 2), TileStatus.Empty);
        }

        [TestMethod]
        public void GetTileStatus_PositiveNumber_ReturnsWall()
        {
            L1GameMap gameMap = new L1GameMap(36, 28);
            Assert.AreEqual(gameMap.GetTileStatus(3, 1), TileStatus.Wall);
            Assert.AreEqual(gameMap.GetTileStatus(33, 2), TileStatus.Wall);
        }

        [TestMethod]
        public void GetTileStatus_PositiveNumber_ReturnsPellet()
        {
            L1GameMap gameMap = new L1GameMap(36, 28);
            Assert.AreEqual(gameMap.GetTileStatus(4, 2), TileStatus.Pellet);
            Assert.AreEqual(gameMap.GetTileStatus(32, 2), TileStatus.Pellet);
        }

        [TestMethod]
        public void GetTileStatus_PositiveNumber_ReturnsPacMan()
        {
            L1GameMap gameMap = new L1GameMap(36, 28);
            Assert.AreEqual(gameMap.GetTileStatus(4, 1), TileStatus.Pacman1);
            Assert.AreEqual(gameMap.GetTileStatus(4, 26), TileStatus.Pacman2);
            Assert.AreEqual(gameMap.GetTileStatus(32, 1), TileStatus.Pacman3);
            Assert.AreEqual(gameMap.GetTileStatus(32, 26), TileStatus.Pacman4);
        }
    }
}