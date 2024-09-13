using Microsoft.AspNetCore.SignalR;
using Server.GameWorld;
using Server.Hubs;
using SharedLibs;

namespace Server.Classes.GameLogic
{
    public class GameLoop
    {
        private readonly IHubContext<GameHub> _hubContext;
        private GameMap _tileStatus;
        public GameLoop(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
            _tileStatus = new GameMap(); 
        }
    }
}
