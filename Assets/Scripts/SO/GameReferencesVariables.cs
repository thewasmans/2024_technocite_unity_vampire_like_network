using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameReferencesVariables", menuName = "SO/GameReferencesVariables", order = 0)]
public class GameReferencesVariables : ScriptableObject
{
    public List<Transform> PlayerTransforms;
}
