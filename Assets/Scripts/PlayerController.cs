using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton
    public static PlayerController instance;

    public float speed;
    public GameObject bulletPrefab;
    public float movementThreshold;

    private Rigidbody2D currentRB;
    private Enemy currentEnemy;
    private int directionMethod;
    private Vector2 pointing;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        currentRB = GetComponent<Rigidbody2D>();
        gm = GameManager.instance;

        var joysticks = Input.GetJoystickNames();
        if (joysticks.Length == 0 || joysticks[0].Length == 0)
        {
            directionMethod = 1;
        }
        else
        {
            directionMethod = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.isPaused)
        {
            return;
        }

        currentRB.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        pointing = determineDirection(pointing);
        if (Input.GetButton("Fire1") && currentEnemy.canShoot)
        {
            StartCoroutine(currentEnemy.ShotCooldown());
            Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<BulletGroup>().direction = pointing;

        }
    }

    private Vector2 determineDirection(Vector2 directionIn)
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
        if (currentEnemy != null)
            currentEnemy.isPlayer = false;

        currentRB = target.GetComponent<Rigidbody2D>();
        currentEnemy = target.GetComponent<Enemy>();
        currentEnemy.isPlayer = true;
    }
}
