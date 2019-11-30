using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// store enemy's attributes
[System.Serializable]
public class TurretData
{
    public GameObject turretPrefab;
    public int cost;
    public GameObject turretPrefab_upgrade;
    public int cost_upgrade;
    public float attackRange;
}
