using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this class is to control the enemy behaviours
public class Enemy : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject poisonEffect;
    public float poisonTime = 0.0f;
    private float poisonDamageRate = 100.0f;
    private float speedPercent = 1.0f;
    public float health;
    private float totalHealth;
    public Slider healthSlider;
    // healSlider = GetComponentInChildren<Slider>();
    public GameObject destroyEffect;
    private Transform[] positions;
    private int index = 0;
    private MonsterBehaviourController behaviourController;
    
    // Start is called before the first frame update
    void Start()
    {
        behaviourController = gameObject.GetComponent<MonsterBehaviourController>();
        positions = WayPoint.positions;
        totalHealth = health;
    }

    // Update is called once per frame
    void Update()
    { 
        if(behaviourController.IsDead() == false)
        {
            if (poisonTime > 0.0f)
            {
                poisonEffect.SetActive(true);
                poisonTime -= Time.deltaTime;
                TakeDamage(Time.deltaTime * poisonDamageRate);
            }
            else
            {
                poisonEffect.SetActive(false);
            }
            Move();
        }
    }

    void Move()
    {
        if (index > positions.Length - 1)
        {
            return;
        }
        else
        {
            Vector3 pos = positions[index].position;
            pos.y = transform.position.y;
            float moveSpeed = speed * speedPercent;
            if (index == 0)
            {
                transform.LookAt(pos);
                this.transform.Translate(Vector3.Normalize(positions[index].position - this.transform.position) * Time.deltaTime * moveSpeed, Space.World);
            }
            else
            {
                transform.LookAt(pos);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            if (Vector3.Distance(pos, transform.position) < 0.1f)
            {
                index++;
            }
        }
        if (index > positions.Length - 1)
        {
            ReachDestination();
        }
    }

    void ReachDestination()
    {
        behaviourController.Attack();
        PlayerDataController.PlayerData.loseHealth(20);
        GameObject.Destroy(this.gameObject,1.5f);
    }
    void OnDestroy()
    {
        EnemySpawner.enemyAliveCount--;
    }
    public void TakeDamage(float damage)
    {
        if (behaviourController.IsDead() == false)
        {
            health -= damage;
            if (health <= 0)
            {
                healthSlider.value = 0;
                poisonTime = 0;
                poisonEffect.SetActive(false);
                EnemyDie();
            }
            else
            {
                healthSlider.value = (float)health / totalHealth;
            }
        }
    }
    public void TakePoison(float time)
    {
        if (poisonTime <= time)
        {
            poisonTime = time;
        }
    }

    public void SpeedDown(float percent)
    {
        if (percent < speedPercent)
        {
            speedPercent = percent;
            foreach (Transform child in transform)
            {
                if (child.CompareTag("MonsterColor"))
                {
                    child.GetComponent<DissolveAppearWithPerlinNoise>().changeColor(Color.blue);
                }
            }
        }
    }

    public void ResetSpeed()
    {
        speedPercent = 1.0f;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("MonsterColor"))
            {
                child.GetComponent<DissolveAppearWithPerlinNoise>().changeColor(Color.white);
            }
        }
    }

    public void GetStunned(float time)
    {
        behaviourController.Stun(time);
    }
    void EnemyDie()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("MonsterColor"))
            {
                child.GetComponent<DissolveAppearWithPerlinNoise>().changeColor(Color.white);
            }
        }
        behaviourController.Die();
        gameObject.tag = "DeadEnemy";
        GameObject buildEffect = GameObject.Instantiate<GameObject>(destroyEffect);
        buildEffect.transform.position = this.transform.position;
        PlayerDataController.PlayerData.addMoney(50);
        Destroy(buildEffect, 2.0f);
        Destroy(this.gameObject, 4.0f);
    }
}
