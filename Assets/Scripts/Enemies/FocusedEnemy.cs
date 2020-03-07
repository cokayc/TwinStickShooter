using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedEnemy : Enemy
{

    GameObject player;

    private Vector3 playerPos;

    protected override void EnemyMovement() 
    {
        player = PlayerController.instance.currentEnemy.gameObject;
        playerPos = player.transform.position - transform.position;
        playerPos.Normalize();

        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime, Space.World);
    }

    public override void Shoot()
    {
        if (isPlayer)
            playerPos = new Vector3(0, 0, 0);
        BulletGroup bullet = Instantiate(bulletGroup, transform.position + playerPos, transform.rotation);
        bullet.GetComponent<BulletGroup>().direction = playerPos;
        bullet.SetShooter(gameObject);
    }
}
