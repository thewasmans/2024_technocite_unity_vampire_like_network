using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class PlayerUpgrades
{
    public static List<PlayerUpgrade> AllUpgrades = new List<PlayerUpgrade>()
    {
        new RangePickupUpgrade(),
        new XpMultiplier(),
        new MoveSpeedUpgrade(),
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
    public abstract void ApplyUpgrade(PlayerMB player);
}

public class RangePickupUpgrade : PlayerUpgrade
{
    private int IncreasePercentage = 20;
    public override string Description => $"Increase XP pickup range by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        var ratio = 1 + IncreasePercentage / (float)100;
        player.XpPickup.localScale *= ratio;
    }
}

public class MoveSpeedUpgrade : PlayerUpgrade
{
    private int IncreasePercentage = 20;
    public override string Description => $"Increase Movespeed by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        var ratio = 1 + IncreasePercentage / (float)100;
        player.Controller.SpeedMovement *= ratio;
    }
}

public class XpMultiplier : PlayerUpgrade
{
    private int IncreasePercentage = 20;
    public override string Description => $"Increase XP by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        var ratio = 1 + IncreasePercentage / (float)100;
        player.PlayerExpHolder.XpMultiplier *= ratio;
    }
}
