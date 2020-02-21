using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static int healthScale = 10;
    private int maxHealth;
    private int currentHealth;
    private float healthPercent;
    private Slider hs;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 0;
        maxHealth = transform.parent.parent.GetComponent<Enemy>().maxHealth;
        if(maxHealth==0)
        {
            Application.ForceCrash(4);
        }
        currentHealth = maxHealth;
        transform.parent.localScale = new Vector3(maxHealth / (float)healthScale, 0.4f, 1);
        hs = GetComponent<Slider>();
        hs.value = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Damage(int damagePoints)
    {
        currentHealth -= damagePoints;
        healthPercent = (float)currentHealth / (float)maxHealth;
        hs.value = healthPercent;
        return currentHealth;
    }
}