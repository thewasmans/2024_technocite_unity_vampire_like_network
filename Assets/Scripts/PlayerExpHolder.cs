using Unity.Netcode;
using UnityEngine;

public class PlayerExpHolder : NetworkBehaviour
{
    private NetworkVariable<float> mExp = new NetworkVariable<float>(0);
    private int mLevel = 1;

    public int LevelDisplay;
    public float ExpDisplay;
    public float XpMultiplier;
    private PlayerMB mPlayerMB;

    void Awake()
    {
        mPlayerMB = GetComponent<PlayerMB>();
    }

    void Start()
    {
        if (!IsServer)
            mExp.OnValueChanged += Refresh;
        XpMultiplier = 1;
    }

    private void Refresh(float previousValue = 0, float newValue = 0)
    {
        UpdateLevel();
        UpdateDisplay();
    }

    internal void AddExp(float expValue)
    {
        mExp.Value += expValue * XpMultiplier;
        Refresh();
    }

    private void UpdateDisplay()
    {
        LevelDisplay = mLevel;
        ExpDisplay = mExp.Value;
    }

    private void UpdateLevel()
    {
        var xpForNextLevel = ExpNextLevel();
        if (mExp.Value >= xpForNextLevel)
        {
            mLevel += 1;
            mExp.Value -= xpForNextLevel;
            mPlayerMB.PlayerUpgradeHandler.OnLevelUpHandler();
        }
    }

    public int ExpNextLevel() => mLevel * 3;

    public float ProgessNextLevel() => mExp.Value / ExpNextLevel();
}
