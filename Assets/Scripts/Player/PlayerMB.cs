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

    public override void OnNetworkSpawn()
    {
        GameVariables.AddPlayerMB(this);
        if (IsLocalPlayer)
            GameVariables.GlobalPlayerXpManager.nvLevel.OnValueChanged +=
                PlayerUpgradeHandler.OnLevelUpHandler;
    }

    public override void OnDestroy()
    {
        if (IsLocalPlayer)
            GameVariables.GlobalPlayerXpManager.nvLevel.OnValueChanged -=
                PlayerUpgradeHandler.OnLevelUpHandler;
        GameVariables.RemovePlayerMB(this);
        base.OnDestroy();
    }
}
