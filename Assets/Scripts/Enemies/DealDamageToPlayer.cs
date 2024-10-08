using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamageToPlayer : MonoBehaviour
{
    private float UpdateTick = 0.2f;
    List<PlayerHealth> playerHealths = new List<PlayerHealth>();
    private bool IsServer;

    void Start()
    {
        IsServer = NetworkManager.Singleton.IsServer;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!IsServer)
            return;
        var pHp = collider.attachedRigidbody?.GetComponent<PlayerHealth>();
        if (pHp == null)
            return;
        playerHealths.Add(pHp);
        UpdateTick = 0;
    }

    void OnTriggerExit(Collider collider)
    {
        if (!IsServer)
            return;
        var pHp = collider.attachedRigidbody?.GetComponent<PlayerHealth>();
        if (pHp == null)
            return;
        playerHealths.Remove(pHp);
    }

    void Update()
    {
        if (!IsServer)
            return;
        UpdateTick -= Time.deltaTime;
        if (UpdateTick > 0)
            return;
        foreach (var pHp in playerHealths)
        {
            pHp.LoseLife();
        }
    }
}
