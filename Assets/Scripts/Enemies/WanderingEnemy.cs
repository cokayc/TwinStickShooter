using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemy : Enemy
{
    public float minPos;
    public float maxPos; 

    private float speed;
    private float wait;
    private float waitTime;
    private bool moving;

    private Vector3 wayPoint;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        waitTime = 0f;
        wait = Random.Range(1f, 3f);
    }

    protected override void EnemyMovement() {
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
            speed = Random.Range(2, 10);
        }
    }

    protected override void Shoot()
    {
        //throw new System.NotImplementedException();
    }
}
