using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly Mock<GameService> _gameServiceMock = new Mock<GameService>();
        private readonly Mock<MessageService> _messageServiceMock = new Mock<MessageService>();
        private readonly PlayerService _playerService;
        private readonly ServiceProvider _provider;

        public GameIntegrationTests() 
        {
            var services = new ServiceCollection();
            services.AddSingleton(_gameServiceMock);
            services.AddSingleton(_messageServiceMock);
            _provider = services.BuildServiceProvider();
            _playerService = new PlayerService(_provider.GetService<GameService>());
        }

        [Fact]
        public async Task PlayerDisconnects_GameResetsIfNoPlayersLeft()
        {
            var gameLoop = new GameLoop(_provider.GetService<GameService>(), _playerService, _provider.GetService<MessageService>(), null, null);
            var hubContext = new Mock<IHubContext<GameHub>>();

            var gameHub = new GameHub(_provider.GetService<MessageService>(), _provider.GetService<GameService>(), _playerService, gameLoop);

            // Simulate player disconnection
            //_playerService.Setup(p => p.GetPlayerCount()).Returns(0);
            await gameHub.OnDisconnectedAsync(null);

            // Verify game reset methods were called
            Assert.Equal(0, _playerService.GetPlayerCount());
        }
    }
}
