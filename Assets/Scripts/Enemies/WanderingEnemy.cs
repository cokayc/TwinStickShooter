using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemy : Enemy
{
    public float minPos;
    public float maxPos; 

    private float wait;
    private float waitTime;

    private Vector3 wayPoint;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        waitTime = 0f;
        wait = Random.Range(1f, 3f);
    }

    protected override void EnemyMovement() {
        /* Commented out to make stationary. Wandering code did not work with level generation setup
        if (moving) 
        {
            transform.Translate((wayPoint - transform.position).normalized * Time.deltaTime);
            if ((wayPoint - transform.position).magnitude < 1) 
            {
                moving = false;
            }
        } 
        else if (waitTime < wait && !moving) 
        {
            waitTime += Time.deltaTime;
        }
        else 
        {
            moving = true;
            waitTime = 0f;
            wayPoint = GeneratePosition(minPos, maxPos);
        }
        */
    }

    public override void Shoot()
    {
        BulletGroup bullet = Instantiate(bulletGroup, transform.position, transform.rotation);
        bullet.GetComponent<BulletGroup>().direction = Vector3.up;
        bullet.SetShooter(gameObject);
    }

 
}
