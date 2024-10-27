using Server.Classes.GameLogic;
using Server.Classes.GameObjects;
using Server.Classes.Services.Builder;
using Server.Classes.Services.Observer;

namespace Server.Classes.Services;

public class GhostService : IResetabbleLoop
{
    private readonly GameService _gameService;
    private GameLoop _gameLoop;

    private List<Ghost> _ghosts = new List<Ghost>();

    public GhostService(GameService gameService)
    {
        _gameService = gameService;
        ((IResetabbleLoop)this).SubscriberToLevelChange();
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
        
        List<GhostBuilder> builders = new List<GhostBuilder>
        {
            new OrangeGhostBuilder(),
            new RedGhostBuilder(),
            new PinkGhostBuilder(),
            new CyanGhostBuilder()
        };
        
        GhostDirector director = new GhostDirector(null);

        for (int i = 0; i < clonedGhosts.Count; i++)
        {
            director = new GhostDirector(builders[i]);
            director.Construct(clonedGhosts[i]);
            clonedGhosts[i] = director.GetGhost();
        }
        
        Ghost shallowCopyGhost = (Ghost)clonedGhosts[2].Clone();
        
        _ghosts.AddRange(clonedGhosts);
        _ghosts.Add(shallowCopyGhost);

        Console.WriteLine($"Blue Ghost hashcode: Hash={_ghosts[0].GetHashCode()}");
        Console.WriteLine($"Red Ghost hashcode: Hash={_ghosts[1].GetHashCode()}");
        Console.WriteLine($"Pink Ghost hashcode: Hash={_ghosts[2].GetHashCode()}");
        Console.WriteLine($"Cyan Ghost hashcode: Hash={_ghosts[3].GetHashCode()}");
        Console.WriteLine($"Shallow copy Ghost hashcode: Hash={_ghosts[4].GetHashCode()} copy ghost Hash={_ghosts[2].GetHashCode()}");

        _ghosts.RemoveAt(4);
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