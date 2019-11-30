using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is used to controll the behaviour of turret
public class TurretController : MonoBehaviour
{

    public List<GameObject> inRangeEnemys = new List<GameObject>();

    // use collider to mimic the attack range
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            inRangeEnemys.Add(other.gameObject);
            if (iceHalo)
            {
                other.gameObject.GetComponent<Enemy>().SpeedDown(SpeedDownPercentage);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            inRangeEnemys.Remove(other.gameObject);
            if (iceHalo)
            {
                other.gameObject.GetComponent<Enemy>().ResetSpeed();
            }
        }
    }


    // properties used to set the type of attack of turrets
    public bool telsla = false;
    public float damageRate;
    private bool telslaEffect = false;
    public bool iceHalo = false;
    public float SpeedDownPercentage;
    public GameObject muzzlePrefab;
    public float attackRateTime; // attack per attackRateTime
    public bool rader = false;
    private bool rederEffectOn = false;
    public LineRenderer raderBeam1;
    public LineRenderer raderBeam2;
    public Transform raderEffect;
    private float timmer = 0.0f;
    public GameObject bulletPrefab; // weapon the turret uses
    public Transform bulletFirePosition;
    public Transform turretHead;
    private void Start()
    {
        timmer += attackRateTime;
    }
    private void Update()
    {
        if (inRangeEnemys.Count > 0 && inRangeEnemys[0] != null && inRangeEnemys[0].GetComponent<MonsterBehaviourController>().IsDead() == false)
        {
            if (telsla == false && rader == false && iceHalo == false)
            {
                Vector3 targetPosition = inRangeEnemys[0].transform.position;
                turretHead.LookAt(targetPosition);
            }
        }
        else
        {
            updateEnemys();
        }
        if (iceHalo == true)
        {
            if (inRangeEnemys.Count > 0)
            {
                foreach (GameObject enemy in inRangeEnemys)
                {
                    enemy.GetComponent<Enemy>().SpeedDown(SpeedDownPercentage);
                }
            }
        }
        else if (telsla == true)
        {
            if (inRangeEnemys.Count > 0)
            {

                if (telslaEffect == false)
                {
                    muzzlePrefab.SetActive(true);
                    telslaEffect = true;
                }

                foreach (GameObject enemy in inRangeEnemys)
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damageRate * Time.deltaTime);
                }
            }
            else
            {
                if (telslaEffect == true)
                {
                    muzzlePrefab.SetActive(false);
                    telslaEffect = false;
                }
            }
        }
        else if (rader == true)
        {
            if(rederEffectOn == false)
            {
                rederEffectOn = true;
                raderBeam1.gameObject.SetActive(true);
            }
            if (inRangeEnemys.Count > 0)
            {
                raderBeam1.SetPositions(new Vector3[] { bulletFirePosition.position, inRangeEnemys[0].transform.position });
                raderBeam2.SetPositions(new Vector3[] { bulletFirePosition.position, inRangeEnemys[0].transform.position });
                inRangeEnemys[0].GetComponent<Enemy>().TakeDamage(damageRate*Time.deltaTime);
                raderEffect.transform.position = inRangeEnemys[0].transform.position;
            }
            else
            {
                if (rederEffectOn == true)
                {
                    rederEffectOn = false;
                    raderBeam1.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            timmer += Time.deltaTime;
            if (timmer >= attackRateTime && inRangeEnemys.Count > 0)
            {
                timmer = 0;
                Attack();
            }
        }

    }

    private void Attack()
    {
        if (inRangeEnemys[0] == null && inRangeEnemys[0].GetComponent<MonsterBehaviourController>().IsDead() == false)
        {
            updateEnemys();
        }
        if (inRangeEnemys.Count > 0)
        {
            if (muzzlePrefab != null)
            {

                GameObject muzzle = GameObject.Instantiate(muzzlePrefab, bulletFirePosition.position, bulletFirePosition.rotation);
                Destroy(muzzle, 1);

            }
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletFirePosition.position, bulletFirePosition.rotation);
            bullet.GetComponent<BulletController>().SetTarget(inRangeEnemys[0]);
        }
        else
        {
            timmer = attackRateTime;
        }
       
    }

    // update inRangeEnemys list to aviod null
    void updateEnemys()
    {
        List<int> invaliIndexs = new List<int>();
        for (int index = 0; index < inRangeEnemys.Count; index++)
        {
            if(inRangeEnemys[index] == null || inRangeEnemys[index].GetComponent<MonsterBehaviourController>().IsDead())
            {
                invaliIndexs.Add(index);
            }
        }

        for (int i = 0; i < invaliIndexs.Count; i++)
        {
            inRangeEnemys.RemoveAt(invaliIndexs[i] - i);
        }
    }
}
