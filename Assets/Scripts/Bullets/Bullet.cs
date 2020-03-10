﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public bool isPossesive;
    public int damage;
    public AudioClip shotSound;
    public AudioClip hitWallSound;
    public AudioClip hitEnemySound;
    [HideInInspector]
    public Vector3 direction;

    private float volume;
    private GameManager gm;
    private GameObject shooter;

    public virtual void Start()
    {
        direction = Vector3.Normalize(direction);
        StartCoroutine(Lifetime());
        gm = GameManager.instance;
        volume = gm.GetEffectsLevel();
        AudioSource.PlayClipAtPoint(shotSound, transform.position, volume);
    }

    public virtual void Update()
    {
        BulletTravel();
    }

    public abstract void BulletTravel();

    private void OnTriggerEnter2D(Collider2D collide)
    {
        GameObject otherObj = collide.gameObject;

        if (otherObj.CompareTag("Enemy")&&DifferentTeams(otherObj, shooter))
        {
            otherObj.GetComponent<Enemy>().Hurt(damage, isPossesive);
            AudioSource.PlayClipAtPoint(hitEnemySound, transform.position, volume);
            Destroy(gameObject);
        }
        else if (otherObj.CompareTag("Wall"))
        {
            AudioSource.PlayClipAtPoint(hitWallSound, transform.position, volume);
            Destroy(gameObject);
        }
    }

    public IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void SetShooter(GameObject s)
    {
        shooter = s;
        if(s.GetComponent<Enemy>().isPlayer&&!isPossesive)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public bool DifferentTeams(GameObject one, GameObject two)
    {
        bool p1 = one.GetComponent<Enemy>().isPlayer;
        bool p2 = two.GetComponent<Enemy>().isPlayer;
        return p1 != p2;
    }

   
}
