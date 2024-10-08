using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUpdateHandler : NetworkBehaviour
{
    private NetworkVariable<bool> nvIsChoosingUpgrade;
    private List<PlayerUpgrade> mUpgradesToChooseFrom;

    void Start()
    {
        nvIsChoosingUpgrade = new NetworkVariable<bool>(
            false,
            writePerm: NetworkVariableWritePermission.Owner
        );
    }

    [Rpc(SendTo.Server)]
    private List<PlayerUpgrade> GetUpgradesRpc()
    {
        return PlayerUpgrades.GetListOfUpgrades(3);
    }

    public void OnLevelUpHandler()
    {
        nvIsChoosingUpgrade.Value = true;

        mUpgradesToChooseFrom = GetUpgradesRpc();
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
