using Unity.Netcode;
using UnityEngine;

public class BulletMb : MonoBehaviour
{
    public float Speed;
    public float Damage;
    public NetworkObject NetworkObject;
    private Transform mTransform;
    public float LifeTime = 10f;
    void Awake()
    {
        mTransform = transform;
    }

    void Start()
    {
        NetworkObject = GetComponent<NetworkObject>();
        // Debug.Log("NetworkObject is null :" + NetworkObject == null);
        // Debug.Log(gameObject.GetComponents<MonoBehaviour>().Length);
        if (NetworkManager.Singleton.IsServer)
            NetworkObject.Spawn();
    }

    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            DestroyBullet();
        mTransform.position += Speed * Time.deltaTime * mTransform.forward;
    }

    void Despawn()
    {
        if (NetworkObject == null)
            return;
        if (!NetworkManager.Singleton.IsServer)
            return;
        if (NetworkObject.IsSpawned)
            NetworkObject.Despawn();
    }

    void OnDestroy()
    {
        Despawn();
    }

    internal void DestroyBullet()
    {
        Despawn();
        Destroy(gameObject);
    }
}
