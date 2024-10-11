using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

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
        mActivableWeapons.OrderBy(aw => Random.Range(0, 1f)).First().ActivateWeapon();
        nvIsGarlicActivated.OnValueChanged += SetActivityOfGarlic;
    }

    private void SetActivityOfGarlic(bool previousValue, bool newValue)
    {
        if (newValue)
            GarlicArea.ActivateWeapon();
        else
            GarlicArea.DeactivateWeapon();
    }

    public override void OnDestroy()
    {
        nvIsGarlicActivated.OnValueChanged -= SetActivityOfGarlic;
        if (IsLocalPlayer)
            GameVariables.GlobalPlayerXpManager.nvLevel.OnValueChanged -=
                PlayerUpgradeHandler.OnLevelUpHandler;
        GameVariables.RemovePlayerMB(this);
        base.OnDestroy();
    }

    internal void ActivateGarlic()
    {
        if (IsServer)
            nvIsGarlicActivated.Value = true;
    }
}
