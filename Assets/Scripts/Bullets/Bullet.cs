using System.Collections;
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

    private AudioSource sound;
    private float volume;
    private GameManager gm;

    public virtual void Start()
    {
        direction = Vector3.Normalize(direction);
        StartCoroutine(Lifetime());
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        volume = gm.GetEffectsLevel();
        sound.volume = volume;
        sound.PlayOneShot(shotSound);
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
            otherObj.GetComponent<AudioSource>().volume = gm.GetEffectsLevel();
            otherObj.GetComponent<AudioSource>().PlayOneShot(hitEnemySound);
            Destroy(gameObject);
        }
        else if (otherObj.CompareTag("Wall"))
        {
            sound.PlayOneShot(hitWallSound);
            StartCoroutine(HitWall());
        }
    }

    public IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public IEnumerator HitWall()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(hitWallSound.length);
        Destroy(gameObject);
    }
}
