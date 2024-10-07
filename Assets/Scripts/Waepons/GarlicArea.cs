using System.Collections.Generic;
using UnityEngine;

public class GarlicArea : MonoBehaviour
{
    public int Damage;
    public float Cooldown;
    public GameReferencesVariables GameVariables;
    public List<EnemyHealth> Enemies;
    private float MaxCooldown;

    private void OnTriggerEnter(Collider other)
    {
        var enemyHealth = other.attachedRigidbody.GetComponent<EnemyHealth>();

        if (enemyHealth)
        {
            Enemies.Add(enemyHealth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemyHealth = other.attachedRigidbody.GetComponent<EnemyHealth>();
        
        if (enemyHealth)
        {
            Enemies.Remove(enemyHealth);
        }
    }

    void Start()
    {
        Cooldown = 0;
    }

    void Update()
    {
        if (Cooldown < MaxCooldown)
        {
            Cooldown += Time.deltaTime;
        }
        else
        {
            foreach (var enemy in Enemies)
            {
                enemy.LoseDamage(Damage);
            }
            Cooldown = 0;
        }
    }
}
