using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject EnemyPrefab;

    [Min(0.1f)]
    public float BaseEnemySpawnTime;

    public ObjectPool<EnemyMover> enemyPool;

    public GameReferencesVariables GameVariables;

    public float SpawnDistance = 3f;
    private float mSpawnTimer;

    public void Awake()
    {
        mSpawnTimer = 0;
        enemyPool = new ObjectPool<EnemyMover>(
            SpawnEnemyPrefab,
            InitNewEnemy,
            DisableEnemy,
            DestroyEnemy,
            defaultCapacity: 500
        );
    }

    public Vector3 GetRandomSpawnPosition()
    {
        var angle = Random.Range(0, Mathf.PI * 2);
        var player = GameVariables.PlayerMBs[Random.Range(0, GameVariables.PlayerMBs.Count)];

        var point =
            player.Transform.position
            + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * SpawnDistance;
        return point;
    }

    public Vector3 GetValidSpawnPosition()
    {
        var distanceSqr = SpawnDistance * SpawnDistance * .99f;
        var pos = Vector3.zero;
        for (var i = 0; i < 20; i++)
        {
            pos = GetRandomSpawnPosition();
            foreach (var player in GameVariables.PlayerMBs)
            {
                if (Vector3.SqrMagnitude(player.Transform.position - pos) < distanceSqr)
                {
                    pos = GetRandomSpawnPosition();
                    break;
                }
            }
        }
        return pos;
    }

    public void SpawnLogic()
    {
        if (!IsServer)
            return;
        mSpawnTimer -= Time.deltaTime;
        if (mSpawnTimer > 0)
            return;
        if (GameVariables.PlayerMBs.Count == 0)
            return;
        SpawnNewEnemy();
    }

    private void SpawnNewEnemy()
    {
        GetNextSpawnTimer();
        // var pos = GetValidSpawnPosition();
        enemyPool.Get();
    }

    EnemyMover SpawnEnemyPrefab()
    {
        return SpawEnnemyPrefab(new Vector3(1000, 1000, 1000));
    }

    EnemyMover SpawEnnemyPrefab(Vector3 pos)
    {
        var enemy = Instantiate(EnemyPrefab, pos, quaternion.identity).GetComponent<EnemyMover>();
        var hp = enemy.GetComponent<EnemyHealth>();
        hp.pool = enemyPool;
        hp.SetHp(0);
        enemy.PlayerMBs = GameVariables.PlayerMBs;

        var no = enemy.GetComponent<NetworkObject>();
        no.Spawn();
        GameVariables.Enemies.Add(enemy.gameObject);
        return enemy;
    }

    void InitNewEnemy(EnemyMover enemy)
    {
        var pos = GetValidSpawnPosition();
        enemy.transform.position = pos;
        enemy.GetComponent<EnemyHealth>().SetHp();
        enemy.gameObject.SetActive(true);
    }

    void DisableEnemy(EnemyMover enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void DestroyEnemy(EnemyMover mover)
    {
        mover.GetComponent<NetworkObject>().Despawn();
        Destroy(mover);
    }

    private void GetNextSpawnTimer()
    {
        mSpawnTimer =
            BaseEnemySpawnTime - (BaseEnemySpawnTime - 0.05f) * Time.time / (Time.time + 100);
    }

    public void Update()
    {
        SpawnLogic();
    }
}
