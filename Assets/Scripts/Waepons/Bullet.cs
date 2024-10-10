using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 1;
    public int Damage = 1;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.attachedRigidbody) return;

        EnemyHealth enemyHealth = other.attachedRigidbody.GetComponent<EnemyHealth>();

        if(!enemyHealth) return;

        enemyHealth.LoseDamage(Damage);
        Destroy(gameObject);
    }
}