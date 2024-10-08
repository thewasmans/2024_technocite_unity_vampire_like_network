using UnityEngine;

public class PlayerMB : MonoBehaviour
{
    public PlayerController Controller;
    public PlayerExpHolder ExpHolder;
    public PlayerHealth Health;
    public Transform TransForm;
    public GameReferencesVariables GameVariables;
    public PlayerUpdateHandler PlayerUpgradeHandler;
    public Transform XpPickup;
    public PlayerExpHolder PlayerExpHolder;

    private void Awake()
    {
        GameVariables.AddPlayerMB(this);
    }

    private void OnDestroy()
    {
        GameVariables.RemovePlayerMB(this);
    }
}
