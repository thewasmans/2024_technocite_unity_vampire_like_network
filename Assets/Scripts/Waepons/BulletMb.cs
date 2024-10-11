using System;
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

    void Update()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            DestroyBullet();
        mTransform.position += Speed * Time.deltaTime * mTransform.forward;
    }

    void OnDestroy()
    {
        NetworkObject?.Despawn();
    }

    internal void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
