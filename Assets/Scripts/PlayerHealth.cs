using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float mImmunityTimer = 2f;
    public float ImmunityDuration = 2f;
    public bool IsImmune => mImmunityTimer > 0;
    public int Hp => mHp;
    public int HpDisplay;

    public bool IsDead { get; private set; }

    private int mHp;

    void Awake()
    {
        mHp = 5;
    }

    void Update()
    {
        mImmunityTimer -= Time.deltaTime;
    }

    public void LoseLife(int dmg = 1)
    {
        if (IsImmune)
            return;
        mImmunityTimer = ImmunityDuration;
        mHp -= dmg;
        HpDisplay = mHp;
        if (Hp <= 0)
        {
            IsDead = true;
            Destroy(gameObject);
        }
    }
}
