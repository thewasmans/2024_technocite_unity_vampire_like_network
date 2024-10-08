using Unity.Netcode;
using UnityEngine;

public class ExpDropper : MonoBehaviour
{
    public GameObject ExpPrefab;

    void OnDestroy()
    {
        if (NetworkManager.Singleton.IsServer)
            DropExp();
    }

    private void DropExp()
    {
        var go = Instantiate(ExpPrefab, transform.position, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
