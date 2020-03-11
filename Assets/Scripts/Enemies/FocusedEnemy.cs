using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedEnemy : Enemy
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
        if (isPlayer)
            bullet.GetComponent<BulletGroup>().direction = new Vector3(-Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad),0);
        else
            bullet.GetComponent<BulletGroup>().direction = playerPos;
        bullet.SetShooter(gameObject);
    }
}
