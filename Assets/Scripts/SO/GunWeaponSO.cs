using UnityEngine;

[CreateAssetMenu(fileName = "GunWeaponSO", menuName = "SO/GunWeaponSO", order = 0)]
public class GunWeaponSO : ScriptableObject
{
    public GameObject BulletPrefab;
    public int NumberOfBulletsInBurst = 3;
    public float TimeBetweenBurst = 1;
    public float BulletDamage = 1;
    public float TimeBetweenBulletsInBurst = .1f;
    public float BulletSpeed = 10f;

    internal void Copy(GunWeaponSO baseSO)
    {
        BulletPrefab = baseSO.BulletPrefab;
        NumberOfBulletsInBurst = baseSO.NumberOfBulletsInBurst;
        TimeBetweenBurst = baseSO.TimeBetweenBurst;
        BulletDamage = baseSO.BulletDamage;
        TimeBetweenBulletsInBurst = baseSO.TimeBetweenBulletsInBurst;
        BulletSpeed = baseSO.BulletSpeed;
    }
}


