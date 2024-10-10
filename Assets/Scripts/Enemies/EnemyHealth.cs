using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHealth : MonoBehaviour
{
    public bool IsAlive { get; private set; }

    public int HitPoint;
    public GameReferencesVariables GameVariables;
    public MeshRenderer Renderer;
    public Material EnemyNormalMaterial;
    public Material EnemyDamagelMaterial;
    internal ObjectPool<EnemyMover> pool;
    private EnemyMover mMover;
    private ExpDropper mExpDropper;

    void Awake()
    {
        mMover = GetComponent<EnemyMover>();
        mExpDropper = GetComponent<ExpDropper>();
    }

    public void LoseDamage(int damage)
    {
        HitPoint -= damage;

        if (HitPoint <= 0)
        {
            HitPoint = -1;
            mExpDropper.LastDeathPosition = transform.position;
            transform.position = Vector3.one * -10000;
            StopAllCoroutines();
            // if (NetworkManager.Singleton.IsServer && !IsAlive)
            //     pool.Release(mMover);
            // Destroy(gameObject);
            IsAlive = false;
        }
        else
        {
            StartCoroutine(AnimateDamage());
        }
    }

    void Update()
    {
        if (NetworkManager.Singleton.IsServer && HitPoint == 0)
            pool.Release(mMover);
        if (!IsAlive)
            HitPoint = 0;
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
    }

    internal void SetHp(int hp = -1)
    {
        if (hp == -1)
            hp = 5 + Mathf.FloorToInt(Time.time) / 20;
        HitPoint = hp;
        IsAlive = HitPoint > 0;
    }
}
