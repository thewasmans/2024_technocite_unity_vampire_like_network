using Unity.Netcode;
using UnityEngine;

public class PlayerExpHolder : NetworkBehaviour
{
    private NetworkVariable<float> mExp = new NetworkVariable<float>(0);
    private int mLevel = 1;

    public int LevelDisplay;
    public float ExpDisplay;

    void Start()
    {
        if (!IsServer)
            mExp.OnValueChanged += Refresh;
    }

    private void Refresh(float previousValue = 0, float newValue = 0)
    {
        UpdateLevel();
        UpdateDisplay();
    }

    internal void AddExp(float expValue)
    {
        mExp.Value += expValue;
        Refresh();
    }

    private void UpdateDisplay()
    {
        LevelDisplay = mLevel;
        ExpDisplay = mExp.Value;
    }

    private void UpdateLevel()
    {
        if(mExp.Value >= ExpNextLevel())
        {
            mLevel =  mLevel + 1;
            mExp.Value = 0;
        }
    }
    
    public int ExpNextLevel() => mLevel * 3;

    public float ProgessNextLevel() => mExp.Value / ((float)ExpNextLevel());
}
