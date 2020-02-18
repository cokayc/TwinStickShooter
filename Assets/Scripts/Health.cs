using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int maxHealth;
    private int currentHealth;
    private float healthPercent;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        transform.parent.localScale = new Vector3(maxHealth/100f, 0.2f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.position = transform.parent.parent.GetComponentInChildren<Rigidbody2D>().gameObject.transform.position + new Vector3(0, 0.8f, 0);
        healthPercent = (float)currentHealth / (float)maxHealth;
        transform.localScale = new Vector3(healthPercent, 1, 1);
        Damage(2);
    }

    public void Damage(int damagePoints)
    {
        currentHealth -= damagePoints;
    }
}
