using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class InGameTimeManager : NetworkBehaviour
{
    public NetworkVariable<bool> IsGamePaused;
    public GameReferencesVariables GameReferencesVariables;

    void Start()
    {
        IsGamePaused = new NetworkVariable<bool>(false);
        IsGamePaused.OnValueChanged += ChangeTimeScale;
    }

    void Update()
    {
        // IsGamePaused.Value = GameReferencesVariables.PlayerTransforms.Any(p => p.IsChoosingUpgrade);
    }

    private void ChangeTimeScale(bool previousValue, bool newValue)
    {
        Time.timeScale = newValue ? 0 : 1;
    }

    void OnDisable()
    {
        IsGamePaused.OnValueChanged -= ChangeTimeScale;
    }
}