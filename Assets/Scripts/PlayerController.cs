using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Singleton
    public static PlayerController instance;

    public bool isADeveloper = false;
    public float speed;
    public float possessiveShotCooldown = 4;
    public float shotCooldownMultiplier;
    public GameObject bulletPrefab;
    public GameObject possessBulletPrefab;

    public float movementThreshold;

    public Image redFlash;
    public Canvas UICanvas;
    public Canvas PauseMenu;
    public Enemy currentEnemy;
    public GameObject startingEnemy;

    private Rigidbody2D currentRB;
    [HideInInspector]
    public int directionMethod;
    private Vector2 pointing;
    private GameManager gm;
    private GameObject mainCamera;

    [HideInInspector]
    public bool instantiated;
    private bool canShootPossessive;
    private bool firstLoadSinceTitle;

    // Start is called before the first frame update
    private void Start()
    {
        instantiated = false;
        canShootPossessive = true;
        gm = GameManager.instance;
    }

    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
        mainCamera = GameObject.Find("Main Camera");
        SceneManager.sceneLoaded += OnLevelLoad;

        var joysticks = Input.GetJoystickNames();
        if (joysticks.Length == 0 || joysticks[0].Length == 0)
            directionMethod = 1;
        else
            directionMethod = 2;
        pointing = Vector3.up;


    }

    // Update is called once per frame
    void Update()
    {
        if (isADeveloper)
        {
            currentEnemy.GetComponentInChildren<Health>().increaseHealth();
            currentEnemy.GetComponentInChildren<Health>().Restore(10);
        }
        if (gm.isPaused||!instantiated||currentEnemy == null||currentRB == null)
        {
            return;
        }
        currentRB.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        pointing = DetermineDirection(pointing);
        if(Input.GetButton("Fire1") && currentEnemy.canShoot)
        {
            StartCoroutine(currentEnemy.ShotCooldown(shotCooldownMultiplier));
            currentEnemy.Shoot();
        }

        if (Input.GetButton("Fire2") && canShootPossessive)
        {
            StartCoroutine(PossessiveCooldown());
            GameObject bullet = Instantiate(possessBulletPrefab, currentRB.gameObject.transform.position, transform.rotation);
            bullet.GetComponent<BulletGroup>().direction = new Vector3(-Mathf.Sin(currentEnemy.gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(currentEnemy.gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 0);
            bullet.GetComponent<BulletGroup>().SetShooter(currentEnemy.gameObject);

        }
    }

    public void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        GameManager.instance.timerText.gameObject.SetActive(true);
        GameManager.instance.scoreText.gameObject.SetActive(true);
        if (scene != SceneManager.GetSceneByName("Level One"))
        {
            GameManager.instance.timerText.gameObject.SetActive(false);
            GameManager.instance.scoreText.gameObject.SetActive(false);
        }
        else if(firstLoadSinceTitle)
        {
            GameManager.instance.timer = 0;
            firstLoadSinceTitle = false;
        }
        mainCamera = GameObject.Find("Main Camera");
        UICanvas.worldCamera = mainCamera.GetComponent<Camera>();
        if(scene == SceneManager.GetSceneByName("Title"))
        {
            UICanvas.gameObject.SetActive(false);
            instantiated = false;
            GameManager.instance.score = 0;
            firstLoadSinceTitle = true;
        }
        else if (scene == SceneManager.GetSceneByName("Level One") || scene == SceneManager.GetSceneByName("Tutorial") || scene == SceneManager.GetSceneByName("Controller Tutorial"))
        {
            UICanvas.gameObject.SetActive(true);
            redFlash.gameObject.SetActive(false);
            //instantiates an enemy controlled by PlayerController at start of level
            if (instantiated)
            {

                //Possess();
            }
            else
            {
                currentEnemy = Instantiate(startingEnemy).GetComponent<Enemy>();
                Possess(currentEnemy.gameObject);
                instantiated = true;
            }
            mainCamera.GetComponent<CameraControl>().target = currentEnemy.gameObject.transform;
        }
        else if (scene == SceneManager.GetSceneByName("Gameover"))
        {
            UICanvas.gameObject.SetActive(false);
            PauseMenu.gameObject.SetActive(false);
        }
    }

    private Vector2 DetermineDirection(Vector2 directionIn)
    {
        Vector2 direction = directionIn;
        switch (directionMethod)
        {
            //Mouse
            case 1:
                Vector2 origin = currentRB.gameObject.transform.position;
                Vector2 target = GetMouseWorldPosition();
                direction = target - origin;
                direction.Normalize();
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                currentRB.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            //PS4
            case 2:
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                if (Mathf.Abs(mouseX) > movementThreshold || Mathf.Abs(mouseY) > movementThreshold)
                {
                    direction = new Vector2(mouseX, mouseY);
                    direction.Normalize();

                    currentRB.gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
                    direction = Vector2.Perpendicular(direction);
                }
                break;
            //XBOX
            case 3:
                break;
        }
        return direction;
    }

    private Vector3 GetMouseWorldPosition()
    {
        // this gets the current mouse position (in screen coordinates) and transforms it into world coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // the camera is on z = -10, so all screen coordinates are on z = -10. To be on the same plane as the game, we need to set z to 0
        mouseWorldPos.z = 0;

        return mouseWorldPos;
    }

    public void Possess(GameObject target)
    {
        currentEnemy.isPlayer = false;
        currentEnemy = target.GetComponent<Enemy>();
        currentRB = target.GetComponent<Rigidbody2D>();
        currentEnemy.isPlayer = true;
        mainCamera.GetComponent<CameraControl>().target = currentEnemy.gameObject.transform;
    }

    public IEnumerator PossessiveCooldown()
    {
        canShootPossessive = false;
        yield return new WaitForSeconds(possessiveShotCooldown);
        canShootPossessive = true;
    }

}
