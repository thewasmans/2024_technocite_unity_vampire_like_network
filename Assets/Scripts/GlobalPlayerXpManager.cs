using Unity.Netcode;
using UnityEngine;

public class GlobalPlayerXpManager : NetworkBehaviour
{
    private NetworkVariable<float> mExp = new NetworkVariable<float>(0);
    public NetworkVariable<int> nvLevel = new NetworkVariable<int>(1);

    public InGameVariablesSO inGameVariablesSO;

    [SerializeField]
    private GameReferencesVariables mGameReferencesVariables;
    private int mLevel => nvLevel.Value;
    public int Level => mLevel;

    void Start()
    {
        mGameReferencesVariables.GlobalPlayerXpManager = this;
        inGameVariablesSO.PlayerXP = 0;
        mExp.OnValueChanged += Refresh;
        mGameReferencesVariables.ActionAddPlayerMB += CatchUpLevels;
    }


    private void CatchUpLevels(PlayerMB mB)
    {
        if (mB.IsLocalPlayer)
            for (int i = 1; i < Level; i++)
                mB.PlayerUpgradeHandler.OnLevelUpHandler();
    }

    private void Refresh(float previousValue = 0, float newValue = 0)
    {
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        var xpForNextLevel = ExpNextLevel();
        if (mExp.Value >= xpForNextLevel)
        {
            nvLevel.Value += 1;
            mExp.Value -= xpForNextLevel;
            // foreach (var p in mGameReferencesVariables.PlayerMBs)
            //     p.PlayerUpgradeHandler.OnLevelUpHandler();
        }
    }

    public int ExpNextLevel() => mLevel * 3;

    public float ProgessNextLevel() => mExp.Value / ExpNextLevel();

    [Rpc(SendTo.Server)]
    internal void AddXPRpc(float v)
    {
        mExp.Value += v;
    }
}
