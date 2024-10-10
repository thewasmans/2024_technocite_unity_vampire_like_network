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
    public GunBullet GunBullet;

    public override void OnNetworkSpawn()
    {
        GameVariables.AddPlayerMB(this);

        if(IsLocalPlayer)
            InitialiseWeaponRpc();
    }

    [Rpc(SendTo.Server)]
    public void InitialiseWeaponRpc() 
    {
        StartCoroutine(GunBullet.FireGun());
    }

    public override void OnDestroy()
    {
        GameVariables.RemovePlayerMB(this);
        base.OnDestroy();
    }
}
