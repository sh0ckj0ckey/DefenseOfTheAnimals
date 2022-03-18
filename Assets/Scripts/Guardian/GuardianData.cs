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

    Portal,         // 传送门
    ChaosMeteor,    // 混沌陨石
    Delete,         // 移除守卫
    Upgrade,        // 升级守卫
}
