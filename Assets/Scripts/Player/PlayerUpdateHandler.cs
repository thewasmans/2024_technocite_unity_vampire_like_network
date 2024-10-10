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
        nvRandomSeedForUpgrades = new NetworkVariable<int>(
            0,
            readPerm: NetworkVariableReadPermission.Owner
        );
        nvRandomSeedForUpgrades.OnValueChanged += GetUpgrades;
        nvIsChoosingUpgrade = new NetworkVariable<bool>(
            false,
            writePerm: NetworkVariableWritePermission.Owner,
            readPerm: NetworkVariableReadPermission.Owner
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
        if (!IsLocalPlayer)
            return;
        mUpgradesToChooseFrom.Add(
            PlayerUpgrades.GetListOfUpgrades(mPlayerMB, 3, nvRandomSeedForUpgrades.Value)
        );
    }

    public void OnLevelUpHandler(int v = 0, int v2 = 0)
    {
        if (IsLocalPlayer)
            GetUpgradesRpc();
    }

    private void ChoseUpgrade(int index)
    {
        mUpgradesToChooseFrom[0][index].ApplyUpgrade(mPlayerMB);
        mUpgradesToChooseFrom.RemoveAt(0);
    }

    void OnGUI()
    {
        if (IsLocalPlayer)
            if (mUpgradesToChooseFrom.Count > 0)
            {
                DrawUpgradePicks();
                nvIsChoosingUpgrade.Value = true;
            }
            else
            {
                nvIsChoosingUpgrade.Value = false;
            }
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
