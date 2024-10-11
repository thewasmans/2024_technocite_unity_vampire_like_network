using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMB : NetworkBehaviour
{
    public PlayerController Controller;
    public PlayerExpHolder ExpHolder;
    public PlayerHealth Health;
    public Transform Transform;
    public GameReferencesVariables GameVariables;
    public PlayerUpdateHandler PlayerUpgradeHandler;
    public Transform XpPickup;
    public PlayerExpHolder PlayerExpHolder;
    public GarlicArea GarlicArea;
    public GunWeaponMB Pistol;
    public GunWeaponMB BurstSmg;
    private List<IActivableWeapon> mActivableWeapons;

    // private NetworkVariable<WeaponUnlocked> mUnlockedWeaponRequest =
    //     new NetworkVariable<WeaponUnlocked>(
    //         WeaponUnlocked.None,
    //         NetworkVariableReadPermission.Owner,
    //         NetworkVariableWritePermission.Owner
    //     );

    private NetworkVariable<WeaponUnlocked> mUnlockedWeaponPermission =
        new NetworkVariable<WeaponUnlocked>(
            WeaponUnlocked.None,
            NetworkVariableReadPermission.Owner,
            NetworkVariableWritePermission.Server
        );
    NetworkVariable<bool> nvIsGarlicActivated = new NetworkVariable<bool>(
        false,
        readPerm: NetworkVariableReadPermission.Owner,
        writePerm: NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        mActivableWeapons = new List<IActivableWeapon>() { Pistol, BurstSmg }; //GarlicArea,
        GameVariables.AddPlayerMB(this);
        if (IsLocalPlayer)
            GameVariables.GlobalPlayerXpManager.nvLevel.OnValueChanged +=
                PlayerUpgradeHandler.OnLevelUpHandler;
        if (IsClient)
        {
            var random = Random.Range(0, 3);
            UnlockWeaponRpc((WeaponUnlocked)Mathf.Pow(2, random));
        }
        mUnlockedWeaponPermission.OnValueChanged += SetActivityForWeapons;
    }

    private void SetActivityForWeapons(WeaponUnlocked previousValue, WeaponUnlocked newValue)
    {
        SetActivityForWeapons(newValue);
    }

    private void SetActivityForWeapons(WeaponUnlocked unlocked)
    {
        if (unlocked.HasFlag(WeaponUnlocked.Smg))
            BurstSmg.ActivateWeapon();
        if (unlocked.HasFlag(WeaponUnlocked.Pistol))
            Pistol.ActivateWeapon();
        if (unlocked.HasFlag(WeaponUnlocked.Garlic))
            GarlicArea.ActivateWeapon();
    }

    [Rpc(SendTo.Server)]
    public void UnlockWeaponRpc(WeaponUnlocked weapon)
    {
        mUnlockedWeaponPermission.Value |= weapon;
        SetActivityForWeapons(mUnlockedWeaponPermission.Value);
    }

    public override void OnDestroy()
    {
        if (IsLocalPlayer)
            GameVariables.GlobalPlayerXpManager.nvLevel.OnValueChanged -=
                PlayerUpgradeHandler.OnLevelUpHandler;
        mUnlockedWeaponPermission.OnValueChanged -= SetActivityForWeapons;
        GameVariables.RemovePlayerMB(this);

        base.OnDestroy();
    }

    internal void ActivateGarlic()
    {
        if (IsServer)
            nvIsGarlicActivated.Value = true;
    }
}

[Flags]
public enum WeaponUnlocked
{
    None = 0,
    Garlic = 1,

    Pistol = 2,
    Smg = 4,
}
