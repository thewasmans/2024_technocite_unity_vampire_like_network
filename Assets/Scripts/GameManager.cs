using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public UnityEditor.SceneAsset UIScene;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
            NetworkManager.Singleton.SceneManager.LoadScene(UIScene.name, LoadSceneMode.Additive);
    }
}