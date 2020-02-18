using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusedEnemy : Enemy
{

    GameObject player;

    void Start()
    {
        //TODO: find in a way that works with possesion.... maybe a gamecontroller keeping track of which object is player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() 
    {

        Vector3 playerPos = player.transform.position - transform.position; 
        playerPos.Normalize();

        float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        FocusedMovement();
    }

    private void FocusedMovement() 
    {
        transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime, Space.World);
    }
}
