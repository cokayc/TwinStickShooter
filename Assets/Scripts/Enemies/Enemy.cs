using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isPlayer;
    public int maxHealth;
    private int health;
    public BulletGroup bulletGroup;

    //generates a random position between min and max
    protected Vector3 GeneratePosition(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), 0.0f);
    }

    private void Update()
    {
        if (!isPlayer)
        {
            EnemyMovement();
        }
    }

    protected abstract void EnemyMovement();

    public void Hurt(int damage, bool isPossessive)
    {
        //if normal bullet
        if (!isPossessive)
        {
            health = GetComponentInChildren<Health>().Damage(damage);
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
        else
        {
            //TODO: possession 
        }
    }

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}