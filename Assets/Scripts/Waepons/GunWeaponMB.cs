using Unity.Netcode;
using UnityEngine;

public class GunWeaponMB : NetworkBehaviour, IActivableWeapon
{
    public GunWeaponSO BaseSO;
    private GunWeaponSO mSO;

    private float InBurstDelay => mSO.TimeBetweenBulletsInBurst;
    private float BurstDelay => mSO.TimeBetweenBurst;
    private int BulletPerBurst => mSO.NumberOfBulletsInBurst;

    public bool IsActivated => mIsActivated;
    public GameObject VisualGo;

    private float mBurstTimer;
    private float mInBurstTimer;
    private int mBulletInCurrentBurst;
    private Transform mTransform;

    [SerializeField]
    private Transform mPlayerVisualMeshTransform;
    private bool mIsActivated;
    public bool IsActivatedDisplay;
    NetworkVariable<bool> nvIsWeaponActivated = new NetworkVariable<bool>(
        false,
        readPerm: NetworkVariableReadPermission.Owner,
        writePerm: NetworkVariableWritePermission.Server
    );

    void Start()
    {
        mSO = ScriptableObject.CreateInstance<GunWeaponSO>();
        mSO.Copy(BaseSO);
        mTransform = transform;
    }

    public void ActivateWeapon()
    {
        mIsActivated = true;
        VisualGo.SetActive(true);
        if (IsServer)
            nvIsWeaponActivated.Value = true;
    }

    void Update()
    {
        IsActivatedDisplay = IsActivated;
        if (!IsActivated)
            return;
        if (mBulletInCurrentBurst < BulletPerBurst)
        {
            mInBurstTimer -= Time.deltaTime;
            if (mInBurstTimer <= 0)
            {
                mBulletInCurrentBurst++;
                mInBurstTimer = InBurstDelay;
                FireRpc(mTransform.position, GetBulletDirection());
            }
        }
        else
        {
            mBurstTimer -= Time.deltaTime;
            if (mBurstTimer <= 0)
            {
                ResetBurst();
            }
        }
    }

    public virtual Quaternion GetBulletDirection()
    {
        return mPlayerVisualMeshTransform.rotation;
    }

    private void ResetBurst()
    {
        mBurstTimer = BurstDelay;
        mInBurstTimer = InBurstDelay;
        mBulletInCurrentBurst = 0;
    }

    [Rpc(SendTo.Server)]
    private void FireRpc(Vector3 pos, Quaternion dir)
    {
        var bulletMb = Instantiate(mSO.BulletPrefab, pos, dir).GetComponent<BulletMb>();
        bulletMb.Damage = mSO.BulletDamage;
        bulletMb.NetworkBehaviour = this;
        bulletMb.Speed = mSO.BulletSpeed;
        var no = bulletMb.GetComponent<NetworkObject>();
        no.Spawn();
    }

    internal void IncreaseFireRate(float ratio) => mSO.TimeBetweenBurst /= 1 + ratio;

    internal void IncreaseDamage(float increaseDamage) => mSO.BulletDamage += increaseDamage;
}

internal interface IActivableWeapon
{
    bool IsActivated { get; }
    void ActivateWeapon();
}
