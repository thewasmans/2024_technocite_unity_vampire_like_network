using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public UnityEditor.SceneAsset UIScene;
    public GameReferencesVariables GameVariables;

    private void Awake()
    {
        GameVariables.ActionAddPlayerMB += LoadUIHUD;
    }

    public void LoadUIHUD(PlayerMB mB)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(UIScene.name, LoadSceneMode.Additive);
        GameVariables.ActionAddPlayerMB -= LoadUIHUD;
    }
}