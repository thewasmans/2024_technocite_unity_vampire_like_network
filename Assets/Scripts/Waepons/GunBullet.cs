using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GunBullet : MonoBehaviour
{
    public Bullet BulletPrefab;
    public Transform StartBullet;
    public GameReferencesVariables GameVariable;
    public int CountBulletPerFire;
    public int SeedRandomFireRate;

    public IEnumerator FireGun()
    {
        while(true)
        {
            for (int i = 0; i < CountBulletPerFire; i++)
            {
                Bullet bullet = Instantiate(BulletPrefab);
                bullet.transform.position = StartBullet.position;
                bullet.transform.rotation = StartBullet.rotation;
                bullet.GetComponent<NetworkObject>().Spawn();
                yield return new WaitForSeconds(.25f + Random.Range(.0f,.25f));
            }
            
            yield return new WaitForSeconds(1);
        }
    }
}
