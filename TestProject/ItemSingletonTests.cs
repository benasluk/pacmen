using Xunit;
using Server.Classes.Services;

public class ItemSingletonTests
{
    [Fact]
    public void GetInstance_ReturnsSameInstance()
    {
        var instance1 = ItemSingleton.getInstance();
        var instance2 = ItemSingleton.getInstance();
        Assert.Same(instance1, instance2);
    }
}
