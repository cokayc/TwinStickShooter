using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject bulletPrefab;
    public float coolDownTime;
    public float movementThreshold;

    private Rigidbody2D myRB;
    private bool canShoot;
    private int directionMethod;
    private Vector2 pointing;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        canShoot = true;
        
        var joysticks = Input.GetJoystickNames();
        foreach (string joystick in joysticks)
        {
            Debug.Log(joystick);
        }
        if(joysticks.Length ==0 || joysticks[0].Length == 0)
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
        myRB.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        pointing = determineDirection(pointing);
        if(Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine(ShotCooldown());
            Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<BulletGroup>().direction=pointing;

        }
    }

    private IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(coolDownTime);
        canShoot = true;
    }

    private Vector2 determineDirection(Vector2 directionIn)
    {
        Vector2 direction = directionIn;
        switch (directionMethod)
        {
            //Mouse
            case 1:
                Vector2 origin = transform.position;
                Vector2 target = GetMouseWorldPosition();
                direction = target - origin;
                direction.Normalize();
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            //PS4
            case 2:
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                if (Mathf.Abs(mouseX)>movementThreshold || Mathf.Abs(mouseY)>movementThreshold)
                {
                    direction = new Vector2(mouseX, mouseY);
                    direction.Normalize();
                    
                    transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
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
}
