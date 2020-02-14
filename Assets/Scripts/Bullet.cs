using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public bool isPossesive;
    public int damage;


    private void Start()
    {
        direction = Vector3.Normalize(direction);
    }

    private void Update()
    {
        Shoot();
    }

    public abstract void Shoot();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject otherObj = collision.collider.gameObject;

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
