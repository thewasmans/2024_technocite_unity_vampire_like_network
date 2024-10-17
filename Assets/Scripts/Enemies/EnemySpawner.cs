using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject EnemyPrefab;

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
        var pos = GetValidSpawnPosition();

        var enemy = Instantiate(EnemyPrefab, pos, quaternion.identity).GetComponent<EnemyMover>();
        enemy.PlayerMBs = GameVariables.PlayerMBs;
        enemy.GetComponent<EnemyHealth>().HitPoint = Mathf.RoundToInt(1 + Time.time * Time.time * .001f);
        var no = enemy.GetComponent<NetworkObject>();
        no.Spawn();
        GameVariables.Enemies.Add(enemy.gameObject);
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
