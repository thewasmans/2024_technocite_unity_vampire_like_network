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
        GameVariables.ActionAddPlayerMB += p => AddPlayer(p);
        
        foreach (var player in GameVariables.PlayerMBs)
        {
            AddPlayer(player);
        }
    }

    public void AddPlayer(PlayerMB player)
    {
        UIPlayerHP playerHP = Instantiate(PrefabPlayerHP);
        playerHP.transform.SetParent(Canvas.transform);
        playerHP.transform.position += Vector3.up * PlayerUIs.Count;

        PlayerUIs.Add(new PlayerUI()
        {
            health = player.Health,
            experience = player.ExpHolder,
            hpUI = playerHP,
        });

        playerHP.SetPlayerName("Player " + PlayerUIs.Count);
    }

    private void Update()
    {
        foreach (var player in PlayerUIs)
        {
            player.hpUI.SetHPValue(player.health.Hp/5.0f);
            
            if(player.experience.IsLocalPlayer)
                PlayerExperience.SetExperienceValue(GameVariables.GlobalPlayerXpManager.ProgessNextLevel());
        }
    }
}