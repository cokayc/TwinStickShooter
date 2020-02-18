﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public BulletGroup bulletGroup;

    //generates a random position between min and max
    protected Vector3 GeneratePosition(float min, float max) 
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), 0.0f);
    }

    public void Hurt(int damage, bool isPossessive)  
    {
        //if normal bullet
        if (!isPossessive) 
        {
            GetComponentInChildren<Health>().Damage(damage);
        } 
        else 
        {
            //TODO: possession 
        }
    }

}
