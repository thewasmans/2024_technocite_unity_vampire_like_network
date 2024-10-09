using Unity.Netcode;
using UnityEngine;

public class PlayerMB : NetworkBehaviour
{
    public PlayerController Controller;
    public PlayerExpHolder ExpHolder;
    public PlayerHealth Health;
    public Transform TransForm;
    public GameReferencesVariables GameVariables;
    public PlayerUpdateHandler PlayerUpgradeHandler;
    public Transform XpPickup;
    public PlayerExpHolder PlayerExpHolder;

    public override void OnNetworkSpawn()
    {
        GameVariables.AddPlayerMB(this);
    }

    public override void OnDestroy()
    {
        GameVariables.RemovePlayerMB(this);
        base.OnDestroy();
    }
}
