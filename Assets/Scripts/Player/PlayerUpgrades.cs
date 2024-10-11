using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public static class PlayerUpgrades
{
    public static List<PlayerUpgrade> AllBaseUpgrades;
    public static List<PlayerUpgrade> GarlicUpgrades = new List<PlayerUpgrade>()
    {
        new GarlicWeaponRangeUpgrade(),
        new GarlicWeaponDamageUpgrade(),
    };

    public static List<PlayerUpgrade> PistolUpgrades = new List<PlayerUpgrade>()
    {
        new PistolWeaponDamageUpgrade(),
        new PistolWeaponFireRateUpgrade(),
    };

    public static List<PlayerUpgrade> BurstSmgUpgrades = new List<PlayerUpgrade>()
    {
        new SmgWeaponDamageUpgrade(),
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
        if (player.GarlicArea.IsActivated)
            availableUpgrade.AddRange(GarlicUpgrades);
        else
            availableUpgrade.Add(new GarlicWeaponUnlockUpgrade());
        if (player.Pistol.IsActivated)
            availableUpgrade.AddRange(PistolUpgrades);
        else
            availableUpgrade.Add(new PistolWeaponUnlockUpgrade());
        if (player.BurstSmg.IsActivated)
            availableUpgrade.AddRange(BurstSmgUpgrades);
        else
            availableUpgrade.Add(new SmgWeaponUnlockUpgrade());
        return availableUpgrade.OrderBy(c => Random.Range(0, 1f)).Take(amount).ToList();
    }
}

#region Weapon Upgrades

#region Pistol upgrades
public class PistolWeaponUnlockUpgrade : PlayerUpgrade
{
    public override string Description => "Unlock Pistol Weapon";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.UnlockWeaponRpc(WeaponUnlocked.Pistol);
    }
}

public class PistolWeaponDamageUpgrade : PlayerUpgrade
{
    private int IncreaseAmount = 3;
    public override string Description => $"Increase Pistol damage by {IncreaseAmount}";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.Pistol.IncreaseDamage(IncreaseAmount);
    }
}

public class PistolWeaponFireRateUpgrade : PlayerUpgrade
{
    private float mRatio => 1 + IncreasePercentage / (float)100;
    private int IncreasePercentage = 20;
    public override string Description => $"Increase Pistol firerate by {IncreasePercentage}%";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.Pistol.IncreaseFireRate(mRatio);
    }
}
#endregion
#region Smg upgrades
public class SmgWeaponUnlockUpgrade : PlayerUpgrade
{
    public override string Description => "Unlock Smg Weapon";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.UnlockWeaponRpc(WeaponUnlocked.Smg);
    }
}

public class SmgWeaponDamageUpgrade : PlayerUpgrade
{
    private int IncreaseAmount = 1;
    public override string Description => $"Increase Smg damage by {IncreaseAmount}";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.BurstSmg.IncreaseDamage(IncreaseAmount);
    }
}
#endregion

#region GarlicUpgrades

public class GarlicWeaponUnlockUpgrade : PlayerUpgrade
{
    public override string Description => "Unlock Garlic Weapon";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.UnlockWeaponRpc(WeaponUnlocked.Garlic);
    }
}

public class GarlicWeaponDamageUpgrade : PlayerUpgrade
{
    private int IncreaseAmount = 1;
    public override string Description => $"Increase Garlic damage by {IncreaseAmount}";

    public override void ApplyUpgrade(PlayerMB player)
    {
        player.GarlicArea.Damage += IncreaseAmount;
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
