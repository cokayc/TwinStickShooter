using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBullet : Bullet
{
    public float minWavePos;
    public float maxWavePos;

    public override void BulletTravel()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
