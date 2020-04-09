using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaggedBullet : Bullet
{
    public float turnTimer;
    public float angle;
    private bool canTurn = false;
    private bool turnSwitch = false;

    public override void Start()
    {
        base.Start();
        StartCoroutine(TurnCooldown());
    }

    public override void BulletTravel()
    {
        if (canTurn)
        {
            if (turnSwitch)
            {
                direction = Quaternion.Euler(0, 0, angle) * direction;
            }
            else
            {
                direction = Quaternion.Euler(0, 0, -angle) * direction;
            }
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 90);
            turnSwitch = !turnSwitch;
            StartCoroutine(TurnCooldown());
        }

        rb.velocity = speed * direction;
    }

    private IEnumerator TurnCooldown()
    {
        canTurn = false;
        yield return new WaitForSeconds(turnTimer);
        canTurn = true;
    }
}
