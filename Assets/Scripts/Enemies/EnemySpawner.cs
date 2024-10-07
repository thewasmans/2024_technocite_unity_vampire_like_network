using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject EnemyPrefab;

    public List<Transform> PlayerPositions => GameVariables.PlayerTransforms;

    [Min(0.1f)]
    public float BaseEnemySpawnTime;

    public GameReferencesVariables GameVariables;

    public float SpawnDistance = 3f;
    private float mSpawnTimer;

    public void Awake()
    {
        mSpawnTimer = 0;
    }

    public Vector3 GetRandomSpawnPosition()
    {
        var angle = Random.Range(0, Mathf.PI * 2);
        var player = PlayerPositions[Random.Range(0, PlayerPositions.Count)];

        var point =
            player.transform.position
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
            foreach (var transform in PlayerPositions)
            {
                if (Vector3.SqrMagnitude(transform.position - pos) < distanceSqr)
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
        if (!IsServer && !IsHost)
            return;
        mSpawnTimer -= Time.deltaTime;
        if (mSpawnTimer > 0)
            return;
        if (PlayerPositions.Count == 0)
            return;
        SpawnNewEnemy();
    }

    private void SpawnNewEnemy()
    {
        GetNextSpawnTimer();
        var pos = GetValidSpawnPosition();

        var enemy = Instantiate(EnemyPrefab, pos, quaternion.identity).GetComponent<EnemyMover>();
        enemy.PlayerTransforms = PlayerPositions;

        var no = enemy.GetComponent<NetworkObject>();
        no.Spawn();
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
