using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GarlicArea : MonoBehaviour, IActivableWeapon
{
    public float Range;
    public Transform Transform;
    public int Damage;
    public float MaxCooldown;
    public GameReferencesVariables GameVariables;
    public List<EnemyHealth> Enemies;
    private float Cooldown;

    public bool IsActivated => gameObject.activeSelf;

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
            foreach (var enemy in Enemies)
            {
                if (!enemy.IsDestroyed())
                    enemy.LoseDamage(Damage);
            }
            Cooldown = 0;
        }
    }

    public void ActivateWeapon()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateWeapon()
    {
        gameObject.SetActive(false);
    }
}
