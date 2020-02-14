using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;

    //generates a random position between min and max
    protected Vector3 GeneratePosition(float min, float max) 
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), 0.0f);
    }

    public void Hurt(int damage, bool bullet)  
    {
        //if normal bullet
        if (!bullet) 
        {
            health -= damage;
        } 
        else 
        {
            //TODO: possession 
        }
    }

}
