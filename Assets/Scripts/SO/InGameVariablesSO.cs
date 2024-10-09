using UnityEngine;

[CreateAssetMenu(fileName = "InGameVariablesSO", menuName = "SO/InGameVariablesSO", order = 0)]
public class InGameVariablesSO : ScriptableObject
{
    public float PlayerXP = 0;

    public void Reset()
    {
        PlayerXP = 0;
    }
}
