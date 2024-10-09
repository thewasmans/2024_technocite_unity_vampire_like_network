using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class PlayerUpgrades
{
    public static List<PlayerUpgrade> AllBaseUpgrades;
    public static List<PlayerUpgrade> GarlicUpgrades = new List<PlayerUpgrade>(){
        new GarlicWeaponRangeUpgrade(),
    };

    static PlayerUpgrades()
    {
        AllBaseUpgrades = CreateAllAvailableUpgradesList();
    }

    private static List<PlayerUpgrade> CreateAllAvailableUpgradesList()
    {
        var list = new List<PlayerUpgrade>()
        {
            new RangePickupUpgrade(),
            new XpMultiplier(),
            new MoveSpeedUpgrade(),
            new IncreaseMaxHpUpgrade(),
            new FullyHealPlayerUpgrade(),
        };
        list.AddRange(GetWeaponUpgrades());
        return list;
    }

    private static IEnumerable<PlayerUpgrade> GetWeaponUpgrades()
    {
        return new List<PlayerUpgrade>();
    }

    public static List<PlayerUpgrade> GetListOfUpgrades(PlayerMB player, int amount, int seed)
    {
        Random.InitState(seed);
        var availableUpgrade = AllBaseUpgrades.ToList();
        if(player.GarlicArea.isActiveAndEnabled)
        availableUpgrade.AddRange(GarlicUpgrades);
        else 
        availableUpgrade.Add(new GarlicWeaponUnlockUpgrade());
        return AllBaseUpgrades.OrderBy(c => Random.Range(0, 1f)).Take(amount).ToList();
    }
}

#region Weapon Upgrades

public class GarlicWeaponUnlockUpgrade : PlayerUpgrade
{
    public override string Description => "Unlock Garlic Weapon";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.GarlicArea.gameObject.SetActive(true);
    }
}

public class GarlicWeaponRangeUpgrade : PlayerUpgrade
{
    private float mRatio => 1 + IncreasePercentage / (float)100;
    private int IncreasePercentage = 20;
    public override string Description => $"Increase Garlic range by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        
        player.GarlicArea.Range *= mRatio;
    }
}

#endregion
#region Player Upgrades
public abstract class PlayerUpgrade
{
    public abstract string Description { get; }
    public abstract void ApplyUpgrade(PlayerMB player);
}

public class FullyHealPlayerUpgrade : PlayerUpgrade
{
    public override string Description => "Fully heal player";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.Health.HealPlayerRpc();
    }
}

public class IncreaseMaxHpUpgrade : PlayerUpgrade
{
    public override string Description => "IncreaseMaxHp";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.Health.AddMaxHpRpc();
    }
}

public class RangePickupUpgrade : PlayerUpgrade
{
    private int IncreasePercentage = 20;
    private float mRatio => 1 + IncreasePercentage / (float)100;

    public override string Description => $"Increase XP pickup range by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.XpPickup.localScale *= mRatio;
    }
}

public class MoveSpeedUpgrade : PlayerUpgrade
{
    private int IncreasePercentage = 20;
    private float mRatio => 1 + IncreasePercentage / (float)100;

    public override string Description => $"Increase Movespeed by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.Controller.SpeedMovement *= mRatio;
    }
}

public class XpMultiplier : PlayerUpgrade
{
    private int IncreasePercentage = 20;
    private float mRatio => 1 + IncreasePercentage / (float)100;

    public override string Description => $"Increase XP by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.PlayerExpHolder.XpMultiplier *= mRatio;
    }
}
#endregion
