using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameReferencesVariables", menuName = "SO/GameReferencesVariables", order = 0)]
public class GameReferencesVariables : ScriptableObject
{
    public List<PlayerMB> PlayerMB;
    public List<GameObject> Enemies;
    
    public event Action<PlayerMB> ActionAddPlayerMB;
    public event Action<PlayerMB> ActionRemovePlayerMB;

    public void AddPlayerMB(PlayerMB playerMB)
    {
        PlayerMB.Add(playerMB);
        ActionAddPlayerMB?.Invoke(playerMB);
    }

    public void RemovePlayerMB(PlayerMB playerMB)
    {
        PlayerMB.Remove(playerMB);
        ActionRemovePlayerMB?.Invoke(playerMB);
    }
}
