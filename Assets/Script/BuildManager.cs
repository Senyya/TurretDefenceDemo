using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// this is the class to control about turret building
// and about attack range showing
public class BuildManager : MonoBehaviour
{
    public TurretData TurretData1;
    public GameObject TurretData1Prototype;
    public TurretData TurretData2;
    public GameObject TurretData2Prototype;
    public TurretData TurretData3;
    public GameObject TurretData3Prototype;
    public TurretData TurretData4;
    public GameObject TurretData4Prototype;
    public TurretData TurretData5;
    public GameObject TurretData5Prototype;
    public TurretData TurretData6;
    public GameObject TurretData6Prototype;
    public TurretData TurretData7;
    public GameObject TurretData7Prototype;
    public int money;
    public Animator moneyAnimator;
    public GameObject upgradeCanvas;
    public Button upgradeBotton;
    // represent the turret data currently choosen
    private TurretData slelectedTurretData;
    private GameObject protoype;
    private GameObject buildtPrototype = null;
    private PlayerDataController playerData;
    // represent the turret currently choosen
    private MapCubeController currentMapCube;
    private Animator upgradeCanvasAnimator;
   
    void Awake()
    {
        playerData = this.gameObject.GetComponent<PlayerDataController>();
        money = playerData.getCurrentMoney();
        upgradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>();
    }
    
    void Update()
    {
        // check if a turret data is chosen
        if (slelectedTurretData != null)
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // if hit on the map, show the turret with its attack range
                    if (hit.transform.name == "GameMap")
                    {
                        if (buildtPrototype != null)
                        {
                            Vector3 pos = hit.point;
                            buildtPrototype.SetActive(true);
                            buildtPrototype.transform.position = pos;
                            pos += new Vector3(0.0f, 1.0f, 0.0f);
                            AttackRangeController1.Instance.gameObject.SetActive(true);
                            AttackRangeController1.Instance.SetPosition(pos);
                            AttackRangeController1.Instance.DrawAttackRange(slelectedTurretData.attackRange);
                        }
                    }
                    // if hit on the map cube, show the turret with its attack range on the map cube
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MapCube"))
                    {
                        MapCubeController mapCube = hit.collider.GetComponent<MapCubeController>();
                        if (mapCube != null)
                        {
                            if (mapCube.currentTurret == null)
                            {
                                if (buildtPrototype != null)
                                {
                                    buildtPrototype.SetActive(true);
                                    buildtPrototype.transform.position = mapCube.GetBuildPosition().position;
                                    AttackRangeController1.Instance.gameObject.SetActive(true);
                                    AttackRangeController1.Instance.SetPosition(buildtPrototype.transform.position);
                                    AttackRangeController1.Instance.DrawAttackRange(slelectedTurretData.attackRange);
                                }
                            }
                            else
                            {
                                if (buildtPrototype != null)
                                {
                                    buildtPrototype.SetActive(false);
                                }
                                AttackRangeController1.Instance.gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        // else, undisplay it
                        if (buildtPrototype != null)
                        {
                            buildtPrototype.SetActive(false);
                        }
                        AttackRangeController1.Instance.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                // else, undisplay it
                if (buildtPrototype != null)
                {
                    buildtPrototype.SetActive(false);
                }
                AttackRangeController1.Instance.gameObject.SetActive(false);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // build turret
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    // here, we can detect the cube which can be built turret on
                    MapCubeController mapCube = hit.collider.GetComponent<MapCubeController>();
                    if (mapCube != null)
                    {
                        if (mapCube.currentTurret == null && slelectedTurretData != null)
                        {
                            // can create turret
                            // check whether have money to create
                            if (playerData.getCurrentMoney() >= slelectedTurretData.cost)
                            {
                                playerData.costMoney(slelectedTurretData.cost);
                                money = playerData.getCurrentMoney();
                                mapCube.BuildTurret(slelectedTurretData);
                            }
                            else
                            {
                                // alert money is not enough
                                moneyAnimator.SetTrigger("flick");
                            }
                        }
                        else if (mapCube.currentTurret != null)
                        {
                            if (mapCube == currentMapCube && upgradeCanvas.activeInHierarchy)
                            {
                                StartCoroutine(undisplayUpgradeMenu());
                                AttackRangeController2.Instance.gameObject.SetActive(false);
                            }
                            else
                            {
                                displayUpgradeMenu(mapCube.transform.position, mapCube.ifUpgraded());
                                AttackRangeController2.Instance.gameObject.SetActive(true);
                                AttackRangeController2.Instance.SetPosition(mapCube.GetBuildPosition().position);
                                AttackRangeController2.Instance.DrawAttackRange(mapCube.turretData.attackRange);
                            }
                            currentMapCube = mapCube;
                        }
                    }
                }
            }
        }
    }

    public void OnTurret1Selection(bool isOn)
    {
        if (isOn)
        {
            slelectedTurretData = TurretData1;
            protoype = TurretData1Prototype;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
            this.buildtPrototype = GameObject.Instantiate<GameObject>(protoype);
        }
        else
        {
            slelectedTurretData = null;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
        }
    }
    public void OnTurret2Selection(bool isOn)
    {
        if (isOn)
        {
            slelectedTurretData = TurretData2;
            protoype = TurretData2Prototype;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
            this.buildtPrototype = GameObject.Instantiate<GameObject>(protoype);
        }
        else
        {
            slelectedTurretData = null;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
        }
    }
    public void OnTurret3Selection(bool isOn)
    {
        if (isOn)
        {
            slelectedTurretData = TurretData3;
            protoype = TurretData3Prototype;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
            this.buildtPrototype = GameObject.Instantiate<GameObject>(protoype);
        }
        else
        {
            slelectedTurretData = null;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
        }
    }
    public void OnTurret4Selection(bool isOn)
    {
        if (isOn)
        {
            slelectedTurretData = TurretData4;
            protoype = TurretData4Prototype;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
            this.buildtPrototype = GameObject.Instantiate<GameObject>(protoype);
        }
        else
        {
            slelectedTurretData = null;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
        }
    }
    public void OnTurret5Selection(bool isOn)
    {
        if (isOn)
        {
            slelectedTurretData = TurretData5;
            protoype = TurretData5Prototype;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
            this.buildtPrototype = GameObject.Instantiate<GameObject>(protoype);
        }
        else
        {
            slelectedTurretData = null;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
        }
    }
    public void OnTurret6Selection(bool isOn)
    {
        if (isOn)
        {
            slelectedTurretData = TurretData6;
            protoype = TurretData6Prototype;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
            this.buildtPrototype = GameObject.Instantiate<GameObject>(protoype);
        }
        else
        {
            slelectedTurretData = null;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
        }
    }
    public void OnTurret7Selection(bool isOn)
    {
        if (isOn)
        {
            slelectedTurretData = TurretData7;
            protoype = TurretData7Prototype;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
            this.buildtPrototype = GameObject.Instantiate<GameObject>(protoype);
        }
        else
        {
            slelectedTurretData = null;
            if (this.buildtPrototype != null)
            {
                Destroy(this.buildtPrototype);
            }
        }
    }

    void displayUpgradeMenu(Vector3 position, bool isDisabled)
    {
        StopCoroutine("undisplayUpgradeMenu");
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = position;
        upgradeBotton.interactable = !isDisabled;
    }
    IEnumerator undisplayUpgradeMenu()
    {
        upgradeCanvasAnimator.SetTrigger("hide");
        yield return new WaitForSeconds(0.6f);
        upgradeCanvas.SetActive(false);
    }

    public void OnUpgradeBottonDown()
    {
        if (playerData.getCurrentMoney() >= currentMapCube.turretData.cost_upgrade)
        {
            playerData.costMoney(currentMapCube.turretData.cost_upgrade);
            money = playerData.getCurrentMoney();
            currentMapCube.UpgradeTurret();
            StartCoroutine(undisplayUpgradeMenu());
            AttackRangeController2.Instance.gameObject.SetActive(false);
        }
        else
        {
            moneyAnimator.SetTrigger("flick");
        }
    }

    public void OnDestroyBottonDown()
    {
        currentMapCube.DestroyTurret();
        StartCoroutine(undisplayUpgradeMenu());
        AttackRangeController2.Instance.gameObject.SetActive(false);
    }
}
