using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is used to set the animation behaviour of monsters
public class MonsterBehaviourController : MonoBehaviour
{
    public string walkAnimation;
    public string stunAnimation;
    public string dieAnimation;
    public string attackAnimation;
    private Animation animationController;
    private float stunTime = 0;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        animationController = this.gameObject.GetComponent<Animation>();
        animationController.Play(walkAnimation);
    }

    void Update()
    {
        if (isDead == false)
        {
            if (stunTime > 0)
            {
                stunTime -= Time.deltaTime;
                if (!animationController.IsPlaying(stunAnimation))
                {
                    animationController.Play(stunAnimation);
                }
            }
            else
            {
                animationController.Play(walkAnimation);
            }
        }
    }

    public float getStunTime()
    {
        return stunTime;
    }
    public bool IsDead()
    {
        return isDead;
    }
    public void Walk()
    {
        animationController.Play(walkAnimation);
    }
    public void Attack()
    {
        isDead = true;
        animationController.Play(attackAnimation);
    }
    public void Stun(float time)
    {
        stunTime += time;
    }

    public void Die()
    {
        isDead = true;
        animationController.Play(dieAnimation);
    }
}
