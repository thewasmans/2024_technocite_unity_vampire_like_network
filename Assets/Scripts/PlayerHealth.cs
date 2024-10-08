using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    private float mImmunityTimer = 2f;
    public float ImmunityDuration = 2f;
    public bool IsImmune => mImmunityTimer > 0;
    public int Hp => mHp.Value;
    public int HpDisplay;

    public bool IsDead { get; private set; }

    private NetworkVariable<int> mHp;

    void Awake()
    {
        mHp = new NetworkVariable<int>(5);
        mHp.OnValueChanged += UpdateHp;
    }

    private void UpdateHp(int previousValue = 0, int newValue = 0)
    {
        HpDisplay = Hp;
        IsDead = Hp <= 0;
    }

    void Update()
    {
        mImmunityTimer -= Time.deltaTime;
    }

    public void LoseLife(int dmg = 1)
    {
        if (!IsServer)
            return;
        if (IsImmune)
            return;
        mImmunityTimer = ImmunityDuration;
        mHp.Value -= dmg;
        UpdateHp();
        if (IsDead)
        {
            Destroy(gameObject);
        }
    }

    public override void OnDestroy()
    {
        mHp.OnValueChanged -= UpdateHp;
    }
}
