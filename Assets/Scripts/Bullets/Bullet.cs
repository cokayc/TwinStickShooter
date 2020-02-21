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

    private float volume;
    private GameManager gm;

    public virtual void Start()
    {
        direction = Vector3.Normalize(direction);
        StartCoroutine(Lifetime());
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
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

        if (otherObj.CompareTag("Enemy"))
        {
            otherObj.GetComponent<Enemy>().Hurt(damage, isPossesive);
            otherObj.GetComponent<AudioSource>().volume = gm.GetEffectsLevel();
            otherObj.GetComponent<AudioSource>().PlayOneShot(hitEnemySound);
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

   
}
