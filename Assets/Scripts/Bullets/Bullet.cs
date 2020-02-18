using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public bool isPossesive;
    public int damage;

    [HideInInspector]
    public Vector3 direction;

    public virtual void Start()
    {
        direction = Vector3.Normalize(direction);
    }

    public virtual void Update()
    {
        Shoot();
    }

    public abstract void Shoot();

    private void OnTriggerEnter2D(Collider2D collide)
    {
        GameObject otherObj = collide.gameObject;

        if (otherObj.CompareTag("Enemy"))
        {
            otherObj.GetComponent<Enemy>().Hurt(damage, isPossesive);
            Destroy(gameObject);
        }
        else if (otherObj.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
