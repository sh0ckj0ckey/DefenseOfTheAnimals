using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuardianData
{
    public GameObject GuardianPrefab;
    public int Cost;
    public int UpgradeCost;
    public GuardianType type;
}

public enum GuardianType
{
    Cactus,         // 仙人刺球
    SoulStream,     // 灵魂激流
    ChainLightning, // 连环闪电
    ChaosMeteor     // 混沌陨石
}
