using Unity.Netcode;
using UnityEngine;

public class ExpMB : MonoBehaviour
{
    public float ExpValue = 1;

    private bool IsServer;

    void Start()
    {
        IsServer = NetworkManager.Singleton.IsServer;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!IsServer)
            return;
        if (!collider.gameObject.CompareTag("Player"))
            return;
        var playerExp = collider.transform.parent.GetComponent<PlayerExpHolder>();
        if (playerExp == null)
            return;
        playerExp.AddExp(ExpValue);
        if (NetworkManager.Singleton.IsServer)
            GetComponent<NetworkObject>().Despawn(true);
    }
}
