using Server.Classes.GameLogic;
using Server.Classes.GameObjects;

namespace Server.Classes.Services;

public class GhostService
{
    private readonly GameService _gameService;
    private GameLoop _gameLoop;

    private List<Ghost> _ghosts = new List<Ghost>();

    public GhostService(GameService gameService)
    {
        _gameService = gameService;
    }

    public void AddGhosts(GameLoop gameLoop)
    {
        if (_gameLoop is null)
            _gameLoop = gameLoop;

        Ghost originalGhost = new Ghost(_gameLoop, _gameService);

        List<Ghost> clonedGhosts = new List<Ghost>
        {
            originalGhost.DeepCopy(),
            originalGhost.DeepCopy(),
            originalGhost.DeepCopy(),
            originalGhost.DeepCopy()
        };
        
        
        Ghost shallowCopyGhost = (Ghost)clonedGhosts[2].Clone();
        
        _ghosts.AddRange(clonedGhosts);
        _ghosts.Add(shallowCopyGhost);
    }

    private void ResetGhosts()
    {
        foreach (var ghost in _ghosts)
        {
            ghost.Destroy();
        }
        
        _ghosts.Clear();
        AddGhosts(_gameLoop);
    }

    public void UpdateGhostsLocations()
    {
        foreach (var ghost in _ghosts)
        {
            ghost.HandleMovement();
        }
    }

    public void ResetAfterLevelChange()
    {
        ResetGhosts();
    }
}