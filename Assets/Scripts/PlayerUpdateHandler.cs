using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUpdateHandler : NetworkBehaviour
{
    private NetworkVariable<bool> nvIsChoosingUpgrade;
    private NetworkVariable<int> nvRandomSeedForUpgrades;
    private List<PlayerUpgrade> mUpgradesToChooseFrom;

    void Start()
    {
        nvRandomSeedForUpgrades = new NetworkVariable<int>(0);
        nvRandomSeedForUpgrades.OnValueChanged += GetUpgrades;
        nvIsChoosingUpgrade = new NetworkVariable<bool>(
            false,
            writePerm: NetworkVariableWritePermission.Owner
        );
    }

    void OnDisable()
    {
        nvRandomSeedForUpgrades.OnValueChanged -= GetUpgrades;
    }

    [Rpc(SendTo.Server)]
    private void GetUpgradesRpc() { }

    private void GetUpgrades(int previousValue = 0, int newValue = 0)
    {
        mUpgradesToChooseFrom = PlayerUpgrades.GetListOfUpgrades(3, nvRandomSeedForUpgrades.Value);
    }

    public void OnLevelUpHandler()
    {
        nvIsChoosingUpgrade.Value = true;

        GetUpgradesRpc();
    }

    private void ChoseUpgrade(int index)
    {
        mUpgradesToChooseFrom[index].ApplyUpgrade();
        nvIsChoosingUpgrade.Value = false;
        mUpgradesToChooseFrom.Clear();
    }

    void OnGUI()
    {
        if (mUpgradesToChooseFrom.Count < 0)
            DrawUpgradePicks();
    }

    private void DrawUpgradePicks()
    {
        for (int i = 0; i < mUpgradesToChooseFrom.Count; i++)
        {
            var upgrade = mUpgradesToChooseFrom[i];

            if (GUI.Button(new Rect(100, 100, 100 + i * 150, 100), upgrade.Description))
            {
                ChoseUpgrade(i);
            }
        }
    }
}