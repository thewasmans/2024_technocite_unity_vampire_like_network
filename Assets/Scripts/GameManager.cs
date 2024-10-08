using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string SceneName;

    private void Awake()
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }    
}