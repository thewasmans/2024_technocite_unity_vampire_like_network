using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerUI
{
    public PlayerHealth health;
    public PlayerExpHolder experience;
    public UIPlayerHP hpUI;
}

public class UI : MonoBehaviour
{
    public GameReferencesVariables GameVariables;
    public Canvas Canvas;
    public UIPlayerHP PrefabPlayerHP;
    public List<PlayerUI> PlayerUIs;
    public UIPlayerExperience PlayerExperience;

    private void Awake()
    {
        GameVariables.ActionAddPlayerMB += AddPlayersAwake;

        foreach (var player in GameVariables.PlayerMBs)
        {
            AddPlayer(player);
        }
    }

    public void AddPlayersAwake(PlayerMB p) => AddPlayer(p);

    public void AddPlayer(PlayerMB player)
    {
        UIPlayerHP playerHP = Instantiate(PrefabPlayerHP);
        playerHP.transform.SetParent(Canvas.transform);
        playerHP.transform.position += Vector3.up * PlayerUIs.Count;

        PlayerUIs.Add(
            new PlayerUI()
            {
                health = player.Health,
                experience = player.ExpHolder,
                hpUI = playerHP,
            }
        );

        playerHP.SetPlayerName("Player " + PlayerUIs.Count);
    }

    private void Update()
    {
        // Debug.Log("UI up");
        foreach (var player in PlayerUIs)
        {
            player.hpUI.SetHPValue(player.health.Hp / (float)player.health.MaxHp); 
            
            PlayerExperience.SetExperienceValue(
                GameVariables.GlobalPlayerXpManager.ProgessNextLevel()
            );
        }
    }

    private void OnDestroy()
    {
        GameVariables.ActionAddPlayerMB -= AddPlayersAwake;
    }
}
