using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static int healthScale = 10;
    public int maxHealth;
    private int currentHealth;
    private float healthPercent;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        transform.parent.localScale = new Vector3(maxHealth/(float) healthScale, 0.4f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        healthPercent = (float)currentHealth / (float)maxHealth;
        GetComponent<Slider>().value = healthPercent;
    }

    public void Damage(int damagePoints)
    {
        currentHealth -= damagePoints;
    }
}
