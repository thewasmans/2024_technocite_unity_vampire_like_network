using System;
using UnityEngine;

public class PlayerExpHolder : MonoBehaviour
{
    private float mExp;
    private int mLevel;

    public int HpDisplayLevel ;
    public float ExpDisplay;

    internal void AddExp(float expValue)
    {
        mExp += expValue;
        UpdateLevel();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        HpDisplayLevel = mLevel;
        ExpDisplay = mExp;
    }

    private void UpdateLevel()
    {
        mLevel = Mathf.RoundToInt(Mathf.Sqrt(mExp));
    }
}
