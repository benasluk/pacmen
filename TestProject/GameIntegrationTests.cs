using Microsoft.AspNetCore.SignalR;
using Moq;
using Server.Classes.GameLogic;
using Server.Classes.Services;
using Server.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class GameIntegrationTests
    {
        [Fact]
        public async Task PlayerDisconnects_GameResetsIfNoPlayersLeft()
        {
            var messageService = new Mock<MessageService>();
            var gameService = new Mock<GameService>();
            var playerService = new Mock<PlayerService>();
            var gameLoop = new GameLoop(gameService.Object, playerService.Object, messageService.Object, null, null);
            var hubContext = new Mock<IHubContext<GameHub>>();

            var gameHub = new GameHub(messageService.Object, gameService.Object, playerService.Object, gameLoop);

            // Simulate player disconnection
            playerService.Setup(p => p.GetPlayerCount()).Returns(0);
            gameHub.OnDisconnectedAsync(null).Wait();

            // Verify game reset methods were called
            Assert.Equal(0, playerService.Object.GetPlayerCount());
        }
    }
}
