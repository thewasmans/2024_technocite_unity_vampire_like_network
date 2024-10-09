using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUpdateHandler : NetworkBehaviour
{
    private NetworkVariable<bool> nvIsChoosingUpgrade;
    private PlayerMB mPlayerMB;
    private NetworkVariable<int> nvRandomSeedForUpgrades;
    private List<List<PlayerUpgrade>> mUpgradesToChooseFrom;
    public bool IsChoosingUpgrade => nvIsChoosingUpgrade.Value;

    void Awake()
    {
        mUpgradesToChooseFrom = new List<List<PlayerUpgrade>>();
        nvRandomSeedForUpgrades = new NetworkVariable<int>(0);
        nvRandomSeedForUpgrades.OnValueChanged += GetUpgrades;
        nvIsChoosingUpgrade = new NetworkVariable<bool>(
            false,
            writePerm: NetworkVariableWritePermission.Owner
        );
        mPlayerMB = GetComponent<PlayerMB>();
    }

    void Start() { }

    void OnDisable()
    {
        nvRandomSeedForUpgrades.OnValueChanged -= GetUpgrades;
    }

    [Rpc(SendTo.Server)]
    private void GetUpgradesRpc()
    {
        nvRandomSeedForUpgrades.Value = Random.Range(0, int.MaxValue);
    }

    private void GetUpgrades(int previousValue = 0, int newValue = 0)
    {
        mUpgradesToChooseFrom.Add(
            PlayerUpgrades.GetListOfUpgrades(3, nvRandomSeedForUpgrades.Value)
        );
    }

    public void OnLevelUpHandler()
    {
        nvIsChoosingUpgrade.Value = true;

        GetUpgradesRpc();
    }

    private void ChoseUpgrade(int index)
    {
        mUpgradesToChooseFrom[0][index].ApplyUpgrade(mPlayerMB);
        nvIsChoosingUpgrade.Value = false;
        mUpgradesToChooseFrom.Clear();
    }

    void OnGUI()
    {
        if (mUpgradesToChooseFrom.Count > 0)
            DrawUpgradePicks();
    }

    private void DrawUpgradePicks()
    {
        for (int i = 0; i < mUpgradesToChooseFrom[0].Count; i++)
        {
            var upgrade = mUpgradesToChooseFrom[0][i];

            if (GUI.Button(new Rect(100 + i * 300, 100, 250, 100), upgrade.Description))
            {
                ChoseUpgrade(i);
                break;
            }
        }
    }
}
