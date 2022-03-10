using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuardianData
{
    public GameObject turretPrefab;
    public int cost;
    public GameObject turretUpgradePrefab;
    public int costUpgraded;
    public TurretType type;
}

public enum TurretType
{
    Normal,
    Advance,
    Super
}
