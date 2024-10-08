using UnityEngine;
using UnityEngine.UI;

public class UIPlayerExperience : MonoBehaviour
{
    [SerializeField]
    private Slider mPlayerExperienceValue;

    public void SetExperienceValue(float value)
    {
        mPlayerExperienceValue.value = value;
    }    
}