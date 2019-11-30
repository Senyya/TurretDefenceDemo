using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// this class is used to controll map cube
public class MapCubeController : MonoBehaviour
{
    [HideInInspector]
    public GameObject currentTurret = null; //store the curret on current cube
    public Transform buildPosition;
    public GameObject buildParticalEffect;
    private MeshRenderer mapRenderer;
    [HideInInspector]
    public TurretData turretData;
    private bool isUpgraded = false;
    private void Start()
    {
        mapRenderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    public bool ifUpgraded()
    {
        return this.isUpgraded;
    }
    // build turret on this map cube
    public void BuildTurret(TurretData turretData)
    {
        if (this.buildPosition != null)
        {
            this.turretData = turretData;
            GameObject turret = GameObject.Instantiate<GameObject>(turretData.turretPrefab);
            turret.transform.position = buildPosition.position;
            currentTurret = turret;
            GameObject buildEffect = GameObject.Instantiate<GameObject>(buildParticalEffect);
            buildEffect.transform.position = buildPosition.position;
            Destroy(buildEffect, 1.5f);
            PlayerDataController.PlayerData.addTurretNumber();
        }
        else
        {
            this.turretData = turretData;
            GameObject turret = GameObject.Instantiate<GameObject>(turretData.turretPrefab);
            turret.transform.position = this.transform.position;
            currentTurret = turret;
            GameObject buildEffect = GameObject.Instantiate<GameObject>(buildParticalEffect);
            buildEffect.transform.position = this.transform.position;
            Destroy(buildEffect, 1.5f);
            PlayerDataController.PlayerData.addTurretNumber();
        }
    }

    // upgrade the turret on this cube
    public void UpgradeTurret()
    {
        if (isUpgraded == true)
        {
            return;
        }
        isUpgraded = true;
        Destroy(currentTurret);
        GameObject turret = GameObject.Instantiate<GameObject>(turretData.turretPrefab_upgrade);
        turret.transform.position = this.buildPosition.position;
        currentTurret = turret;
        GameObject buildEffect = GameObject.Instantiate<GameObject>(buildParticalEffect);
        buildEffect.transform.position = this.buildPosition.position;
        Destroy(buildEffect, 1.5f);
    }

    public void DestroyTurret()
    {
        if (currentTurret.GetComponent<TurretController>().iceHalo == true)
        {
            foreach (GameObject oneEnemy in currentTurret.GetComponent<TurretController>().inRangeEnemys)
            {
                oneEnemy.GetComponent<Enemy>().ResetSpeed();
            }
            Destroy(currentTurret);
            PlayerDataController.PlayerData.decreaseTurretNumber();
        }
        else
        {
            Destroy(currentTurret);
            PlayerDataController.PlayerData.decreaseTurretNumber();
        }
        currentTurret = null;
        isUpgraded = false;
        turretData = null;
        GameObject buildEffect = GameObject.Instantiate<GameObject>(buildParticalEffect);
        buildEffect.transform.position = this.transform.position;
        PlayerDataController.PlayerData.addMoney(30);
        Destroy(buildEffect, 1.5f);
    }

    public Transform GetBuildPosition()
    {
        return buildPosition;
    }

    private void OnMouseEnter()
    {
        if (currentTurret == null && EventSystem.current.IsPointerOverGameObject() == false)
        {
            mapRenderer.material.color = Color.grey;
        }

    }
    private void OnMouseExit()
    {
        mapRenderer.material.color = Color.white;
    }
}
