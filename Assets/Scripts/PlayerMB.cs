using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMB : MonoBehaviour
{
    public PlayerController Controller;
    public PlayerExpHolder ExpHolder;
    public PlayerHealth Health;
    public Transform TransForm;
    public GameReferencesVariables GameVariables;

    private void Awake()
    {
        GameVariables.AddPlayerMB(this);
    }

    private void OnDestroy()
    {
        GameVariables.RemovePlayerMB(this);
    }
}