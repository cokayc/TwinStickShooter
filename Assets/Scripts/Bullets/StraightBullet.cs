using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : Bullet
{


    public override void BulletTravel()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
