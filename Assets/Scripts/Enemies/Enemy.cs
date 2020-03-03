using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    public Sprite[] deathSprites;
    public bool isPlayer;
    public int maxHealth;
    public AudioClip deathSound;
    public float shotCooldown;
    public BulletGroup bulletGroup;

    private int health;

    [HideInInspector]
    public bool canShoot;

    //generates a random position between min and max
    protected Vector3 GeneratePosition(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), 0.0f);
    }

    private void Awake()
    {
        canShoot = true;
    }

    private void Update()
    {
        if (!isPlayer)
        {
            EnemyMovement();
            if (canShoot)
            {
                Shoot();
                StartCoroutine(ShotCooldown());
            }
        }
    }

    protected abstract void Shoot();
    protected abstract void EnemyMovement();

    public void Hurt(int damage, bool isPossessive)
    {
        //if normal bullet
        if (!isPossessive)
        {
            health = GetComponentInChildren<Health>().Damage(damage);
            if(isPlayer)
            {
                StartCoroutine(PlayerHurt());
            }
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
        else
        {
            PlayerController.instance.Possess(gameObject);
        }
    }

    public IEnumerator Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        int n = 0;
        int k = 0;
        while(k<deathSprites.Length)
        {
            GetComponent<SpriteRenderer>().sprite = deathSprites[k];
            while(n<5)
            {
                yield return null;
                n++;
            }
            n = 0;
            k++;
        }
        Destroy(gameObject);
    }

    public IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotCooldown);
        canShoot = true;
    }

    public IEnumerator PlayerHurt()
    {
        PlayerController.instance.redFlash.gameObject.SetActive(true);
        CameraControl.ScreenShakeStrong();
        yield return new WaitForSeconds(0.5f);
        PlayerController.instance.redFlash.gameObject.SetActive(false);
    }

}