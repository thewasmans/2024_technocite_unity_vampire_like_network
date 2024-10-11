using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int HitPoint;
    public GameReferencesVariables GameVariables;
    public MeshRenderer Renderer;
    public Material EnemyNormalMaterial;
    public Material EnemyDamagelMaterial;

    public void LoseDamage(int damage)
    {
        HitPoint -= damage;

        if (HitPoint <= 0)
        {
            if (NetworkManager.Singleton.IsServer)
                GetComponent<NetworkObject>().Despawn(true);
            // Destroy(gameObject);
        }

        StartCoroutine(AnimateDamage());
    }

    public IEnumerator AnimateDamage()
    {
        Renderer.material = EnemyDamagelMaterial;
        yield return new WaitForSeconds(.1f);
        Renderer.material = EnemyNormalMaterial;
        yield return new WaitForSeconds(.1f);
        Renderer.material = EnemyDamagelMaterial;
        yield return new WaitForSeconds(.1f);
        Renderer.material = EnemyNormalMaterial;
    }

    public void OnDestroy()
    {
        GameVariables.Enemies.Remove(gameObject);
        StopAllCoroutines();
    }

    void OnTriggerEnter(Collider collider)
    {
        var bullet = collider.transform.parent.parent.GetComponent<BulletMb>();
        if (bullet == null)
            return;
        LoseDamage(Mathf.RoundToInt(bullet.Damage));
        bullet.DestroyBullet();
    }
}
