using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : Bullet
{

    public override void BulletTravel()
    {
        rb.velocity = speed * direction;
    }
}
