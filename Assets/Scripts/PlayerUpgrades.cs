using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class PlayerUpgrades
{
    public static List<PlayerUpgrade> AllUpgrades = new List<PlayerUpgrade>()
    {
        new RangePickupUpgrade(),
    };

    public static List<PlayerUpgrade> GetListOfUpgrades(int amount, int seed)
    {
        Random.InitState(seed);
        return AllUpgrades.OrderBy(c => Random.Range(0, 1f)).Take(amount).ToList();
    }
}

public abstract class PlayerUpgrade
{
    public abstract string Description { get; }
    public abstract void ApplyUpgrade();
}

public class RangePickupUpgrade : PlayerUpgrade
{
    public override string Description => throw new System.NotImplementedException();

    public override void ApplyUpgrade()
    {
        throw new System.NotImplementedException();
    }
}
