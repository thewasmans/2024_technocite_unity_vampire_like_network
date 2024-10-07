using System.Collections.Generic;
using UnityEngine;

public class DealDamageToPlayer : MonoBehaviour
{
    private float UpdateTick = 0.2f;
    List<PlayerHealth> playerHealths = new List<PlayerHealth>();

    void OnTriggerEnter(Collider collider)
    {
        var pHp = collider.attachedRigidbody.gameObject.GetComponent<PlayerHealth>();
        if (pHp == null)
            return;
        playerHealths.Add(pHp);
        UpdateTick = 0;
    }

    void OnTriggerExit(Collider collider)
    {
        var pHp = collider.attachedRigidbody.gameObject.GetComponent<PlayerHealth>();
        if (pHp == null)
            return;
        playerHealths.Remove(pHp);
    }

    void Update()
    {
        UpdateTick -= Time.deltaTime;
        if (UpdateTick > 0)
            return;
        foreach (var pHp in playerHealths)
        {
            pHp.LoseLife();
        }
    }
}
