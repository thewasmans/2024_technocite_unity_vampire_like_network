using Unity.Netcode;
using UnityEngine;

public class PlayerExpHolder : NetworkBehaviour
{
    private NetworkVariable<float> mExp = new NetworkVariable<float>(0);
    private int mLevel;

    public int HpDisplayLevel;
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
        HpDisplayLevel = mLevel;
        ExpDisplay = mExp.Value;
    }

    private void UpdateLevel()
    {
        mLevel = Mathf.RoundToInt(Mathf.Sqrt(mExp.Value));
    }
}
