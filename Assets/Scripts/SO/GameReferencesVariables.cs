using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameReferencesVariables", menuName = "SO/GameReferencesVariables", order = 0)]
public class GameReferencesVariables : ScriptableObject
{
    public List<PlayerMB> PlayerMBs;
    public List<GameObject> Enemies;
    
    public event Action<PlayerMB> ActionAddPlayerMB;
    public event Action<PlayerMB> ActionRemovePlayerMB;

    public void AddPlayerMB(PlayerMB playerMB)
    {
        PlayerMBs.Add(playerMB);
        ActionAddPlayerMB?.Invoke(playerMB);
    }

    public void RemovePlayerMB(PlayerMB playerMB)
    {
        PlayerMBs.Remove(playerMB);
        ActionRemovePlayerMB?.Invoke(playerMB);
    }
}
