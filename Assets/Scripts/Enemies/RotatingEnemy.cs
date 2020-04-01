using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemy : Enemy
{
    GameObject player;
    private Vector3 playerPos;

    private void Start()
    {
        moving = true;
    }
    protected override void EnemyMovement()
    {
        player = PlayerController.instance.currentEnemy.gameObject;
        playerPos = player.transform.position - transform.position;
        float dist = Vector3.Distance(playerPos, transform.position);

        playerPos.Normalize();

        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (dist > 10)
        {
            transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime, Space.World);
        } else 
        {
            transform.Translate(-(player.transform.position - transform.position).normalized * Time.deltaTime, Space.World);
        }
        transform.RotateAround(player.transform.position, Vector3.forward, 30 * Time.deltaTime);

    }

    public override void Shoot()
    {
        if (isPlayer)
            playerPos = new Vector3(0, 0, 0);
        BulletGroup bullet = Instantiate(bulletGroup, transform.position + playerPos, transform.rotation);
        if (isPlayer)
            bullet.GetComponent<BulletGroup>().direction = new Vector3(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 0);
        else
            bullet.GetComponent<BulletGroup>().direction = playerPos;
        bullet.SetShooter(gameObject);
    }
}
