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
        FocusedMovement();
    }

    private void FocusedMovement() 
    {
        transform.Translate((player.transform.position - transform.position).normalized * Time.deltaTime);
    }
}
