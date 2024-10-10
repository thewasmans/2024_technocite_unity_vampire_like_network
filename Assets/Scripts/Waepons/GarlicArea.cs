using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GarlicArea : MonoBehaviour
{
    public float Range;
    public Transform Transform;
    public int Damage;
    public float MaxCooldown;
    public GameReferencesVariables GameVariables;
    public List<EnemyHealth> Enemies;
    private float Cooldown;

    private void OnTriggerEnter(Collider other)
    {
        var enemyHealth = other.attachedRigidbody?.GetComponent<EnemyHealth>();

        if (enemyHealth)
        {
            Enemies.Add(enemyHealth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemyHealth = other.attachedRigidbody?.GetComponent<EnemyHealth>();

        if (enemyHealth)
        {
            Debug.Log("left garlic");
            Enemies.Remove(enemyHealth);
        }
    }

    void Awake()
    {
        Transform = transform;
    }

    void Start()
    {
        Cooldown = 0;
    }

    void Update()
    {
        Transform.localScale = Vector3.one * Range;
        if (Cooldown < MaxCooldown)
        {
            Cooldown += Time.deltaTime;
        }
        else
        {
            var inactiveEnemies = Enemies.Where(x => !x.IsAlive).ToList();
            for (int i = 0; i < inactiveEnemies.Count; i++)
                Enemies.Remove(inactiveEnemies[i]);
            foreach (var enemy in Enemies)
            {
                if (enemy.gameObject.activeSelf)
                    enemy.LoseDamage(Damage);
            }
            Cooldown = 0;
        }
    }
}
