using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static int healthScale = 10;
    private int maxHealth;
    private int currentHealth;
    [HideInInspector]
    public float healthPercent;
    private Slider hs;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 0;
        maxHealth = transform.parent.parent.GetComponent<Enemy>().maxHealth;
        if(maxHealth==0)
        {
            Debug.Log("We really messed it up");
        }
        currentHealth = maxHealth;
        transform.parent.localScale = new Vector3(maxHealth / (float)healthScale, 0.4f, 1);
        hs = GetComponent<Slider>();
        hs.value = 1;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.parent.localRotation = Quaternion.Euler(new Vector3(0,0,-transform.parent.parent.rotation.eulerAngles.z));
        transform.parent.localPosition = FindOneUnitAbove(transform.parent.localRotation);
    }

    public int Damage(int damagePoints)
    {
        currentHealth -= damagePoints;
        healthPercent = (float)currentHealth / (float)maxHealth;
        hs.value = healthPercent;
        return currentHealth;
    }

    //restore health for the player
    public int Restore(int healthpoints)
    {
        if(currentHealth + healthpoints > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
            currentHealth += healthpoints;
        healthPercent = (float)currentHealth / (float)maxHealth;
        hs.value = healthPercent;
        return currentHealth;

    }



    public Vector2 FindOneUnitAbove(Quaternion direction)
    {
        float angle = -direction.eulerAngles.z*Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }


    public void increaseHealth()
    {
        maxHealth += 10;
        currentHealth += 10;
    }
}