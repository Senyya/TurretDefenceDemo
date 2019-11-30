using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the class to define the property of bullet
// poison means it this bullert has poison
public class BulletController : MonoBehaviour
{
    public int damage = 50;
    public float speed = 20.0f;
    public bool poison = false;
    public float poisonTime = 0.0f;
    public GameObject bulletDestroyEffect;
    private GameObject target;

    // Update is called once per frame
    void Update()
    {
        if (target.transform != null && target.GetComponent<MonsterBehaviourController>().IsDead() == false)
        {
            transform.LookAt(target.transform.position);
            this.transform.Translate(Vector3.Normalize(target.transform.position - this.transform.position) * Time.deltaTime * speed, Space.World);
        }
        else
        {
            Die();
        }
    }
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            if (poison == true)
            {
                other.GetComponent<Enemy>().TakePoison(poisonTime);
            }
            Die();
        }
    }
    void Die()
    {
        GameObject effect = GameObject.Instantiate(bulletDestroyEffect, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
        Destroy(effect, 1);
    }
}
