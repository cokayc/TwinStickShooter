using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class Enemy : MonoBehaviour
{
    public bool isPlayer;
    public int maxHealth;
    public int scoreValue;
    public AudioClip deathSound;
    public float shotCooldown;
    public BulletGroup bulletGroup;

    private int health;

    private Rigidbody2D rb2d;

    [HideInInspector]
    public bool canShoot;

    //animation variables
    public Sprite[] moveSprites;
    public Sprite[] deathSprites;
    public int framesPerSecond;
    private int currentFrameIndex;
    private SpriteRenderer sr;
    protected bool moving;
    private bool isDying;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        canShoot = true;
        StartCoroutine("PlayAnimation");
        isDying = false;
    }

    private void Update()
    {
        if (isDying)
            return;
        //handle movement of this enemy, unless it is being controlled by the player
        //in that case, PlayerController handles the movement and shooting of this enemy
        if (!isPlayer)
        {
            EnemyMovement();
            if (canShoot)
            {
                Shoot();
                StartCoroutine(ShotCooldown());
            }
        }
        else
        {
            //determines whether player is moving enemy, used for animation
            if (rb2d == null || rb2d.velocity.magnitude == 0)
            {
                moving = false;
            } else
            {
                moving = true;
            }
        }
    }

    //generates a random position between min and max
    protected Vector3 GeneratePosition(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), 0.0f);
    }

    public abstract void Shoot();
    protected abstract void EnemyMovement();

    //if isPossessive is false, deals damage to enemy. if isPossessive is true, causes possession  
    public void Hurt(int damage, bool isPossessive)
    {
        if (isDying)
            return;
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

    private void OnTriggerEnter2D(Collider2D collide)
    {
        GameObject otherObj = collide.gameObject;
        if (otherObj.CompareTag("Heart"))
        {
            health = GetComponentInChildren<Health>().Restore(1);
            Destroy(otherObj);
        }
        else if (otherObj.CompareTag("Exit") && isPlayer)
        {
            DontDestroyOnLoad(gameObject);
            GameManager.instance.NextLevel();
        }
    }

    public IEnumerator Die()
    {
        isDying = true;
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        int n = 0;
        Destroy(rb2d);
        Destroy(GetComponent<Collider2D>());
        //play death animation
        for (int k = 0;  k < deathSprites.Length; k++)
        {
            canShoot = false;
            sr.sprite = deathSprites[k];
            while (n < 5)
            {
                yield return null;
                n++;
            }
            n = 0;
        }
        if (isPlayer)
            SceneManager.LoadScene("Gameover");
        else
            GameManager.instance.score += scoreValue;
        Destroy(gameObject);
    }

    public IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotCooldown);
        canShoot = true;
    }

    public IEnumerator ShotCooldown(float multiplier)
    {
        canShoot = false;
        yield return new WaitForSeconds(shotCooldown * multiplier);
        canShoot = true;
    }

    public IEnumerator PlayerHurt()
    {
        PlayerController.instance.redFlash.gameObject.SetActive(true);
        CameraControl.ScreenShake(0.5f, 0.3f);
        yield return new WaitForSeconds(0.5f);
        PlayerController.instance.redFlash.gameObject.SetActive(false);
    }


    IEnumerator PlayAnimation()
    {
        while (!isDying)
        {
            yield return new WaitForSeconds(1f / framesPerSecond);

            if (moving)
            {
                if (currentFrameIndex >= moveSprites.Length)
                {
                    currentFrameIndex = 0;
                }
                sr.sprite = moveSprites[currentFrameIndex];
                currentFrameIndex++;
            }
        }
    }

}