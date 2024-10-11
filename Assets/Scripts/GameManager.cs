using Unity.Netcode;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class GameManager : NetworkBehaviour
{
    public Object Scene;
    public string SceneName;
    
    public GameReferencesVariables GameVariables;

    private void Awake()
    {
        GameVariables.ActionAddPlayerMB += LoadUIHUD;
    }

    public void LoadUIHUD(PlayerMB mB)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        GameVariables.ActionAddPlayerMB -= LoadUIHUD;
    }
}