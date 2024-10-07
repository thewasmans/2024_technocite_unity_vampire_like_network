using System;
using UnityEngine;

public class PlayerExpHolder : MonoBehaviour
{
    private float mExp;
    private int mLevel;

    public int HpDisplayLevel => mLevel;
    public float ExpDisplay => mExp;

    internal void AddExp(float expValue)
    {
        mExp += expValue;
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        mLevel = Mathf.RoundToInt(Mathf.Sqrt(mExp));
    }
}
