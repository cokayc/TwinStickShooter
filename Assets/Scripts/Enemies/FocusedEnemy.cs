using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedEnemy : Enemy
{

    GameObject player;

    private Vector3 playerPos;

    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(FireBullets());
    }

    protected override void EnemyMovement() 
    {
        playerPos = player.transform.position - transform.position;
        playerPos.Normalize();

        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime, Space.World);
    }

    IEnumerator FireBullets()
    {
        while (!isPlayer)
        {
            yield return new WaitForSeconds(5.0f);

            Instantiate(bulletGroup, transform.position+playerPos, transform.rotation).GetComponent<BulletGroup>().direction = playerPos;
        }
    }
}
