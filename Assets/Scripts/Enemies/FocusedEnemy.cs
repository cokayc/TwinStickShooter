using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedEnemy : Enemy
{

    GameObject player;

    private Vector3 playerPos;

    void Start()
    {
        //TODO: find in a way that works with possesion.... maybe a gamecontroller keeping track of which object is player
        player = GameObject.Find("Player");
        StartCoroutine(FireBullets());
    }

    // Update is called once per frame
    void Update() 
    {

        playerPos = player.transform.position - transform.position; 
        playerPos.Normalize();

        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        FocusedMovement();
    }

    private void FocusedMovement() 
    {
        transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime, Space.World);
    }

    IEnumerator FireBullets()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);

            Instantiate(bulletGroup, transform.position+playerPos, transform.rotation).GetComponent<BulletGroup>().direction = playerPos;
        }
    }
}
