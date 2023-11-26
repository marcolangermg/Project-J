using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum itemType
{
    Peitoral,
    Capacete,
    Botas,
    Luvas,
    Bracadeiras,
    Perneiras,
    ArmaUmaMao,
    ArmaDuasMaos
}

public class item : MonoBehaviour
{
    public string itemName;
    public int itemID;
    public itemType itemType;
    public float fireResistance = 0.0f;
    public float waterResistance = 0.0f;
    public float earthResistance = 0.0f;
    public float airResistance = 0.0f;
    public float energyResistance = 0.0f;
    public float physicalResistance = 0.0f;
}