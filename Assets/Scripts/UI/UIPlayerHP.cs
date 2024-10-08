using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    [SerializeField]
    private Slider mPlayerHPValue;
    
    [SerializeField]
    private Text mPlayerName;

    public void SetHPValue(float value)
    {
        mPlayerHPValue.value = value;
    }

    public void SetPlayerName(string name)
    {
        mPlayerName.text = name;
    } 
}