using Unity.Netcode;
using UnityEngine;

public class PlayerExpHolder : NetworkBehaviour
{
    public int LevelDisplay => mGameRefs.GlobalPlayerXpManager.Level;
    public float XpMultiplier;

    [SerializeField]
    private GameReferencesVariables mGameRefs;

    void Start()
    {
        XpMultiplier = 1;
    }

    internal void AddExp(float expValue)
    {
        mGameRefs.GlobalPlayerXpManager.AddXP(expValue * XpMultiplier);
    }
}
