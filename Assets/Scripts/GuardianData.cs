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
    Cactus,         // ÏÉÈË´ÌÇò
    SoulStream,     // Áé»ê¼¤Á÷
    ChainLightning, // Á¬»·ÉÁµç

    ChaosMeteor,    // »ìãçÔÉÊ¯
    Delete,         // ÒÆ³ıÊØÎÀ
    Upgrade,        // Éı¼¶ÊØÎÀ
}
