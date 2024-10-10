using Unity.Netcode;
using UnityEngine;

public class ExpDropper : MonoBehaviour
{
    public GameObject ExpPrefab;
    public Vector3 LastDeathPosition;

    void OnDisable()
    {
        if (NetworkManager.Singleton.IsServer)
            DropExp();
    }

    private void DropExp()
    {
        var go = Instantiate(ExpPrefab, LastDeathPosition, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
