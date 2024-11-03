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
        private readonly ServiceProvider _provider;

        public GameIntegrationTests()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_gameServiceMock);
            services.AddSingleton(_messageServiceMock);
            _provider = services.BuildServiceProvider();
        }
    }
}
