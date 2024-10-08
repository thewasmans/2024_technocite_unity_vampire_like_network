using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int HitPoint;
    public GameReferencesVariables GameVariables;

    public void LoseDamage(int damage)
    {
        HitPoint -= damage;

        if (HitPoint <= 0)
        {
            if (NetworkManager.Singleton.IsServer)
                GetComponent<NetworkObject>().Despawn(true);
            // Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        GameVariables.Enemies.Remove(gameObject);
    }
}
