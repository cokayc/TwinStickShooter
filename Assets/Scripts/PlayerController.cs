using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject bulletPrefab;
    public float coolDownTime;

    private Rigidbody2D myRB;
    private bool canShoot;
    private int directionMethod;
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
        if(joysticks[0].Length == 0)
        {
            directionMethod = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        myRB.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        determineDirection();
        if(Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine(ShotCooldown());
            //Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>().direction=transform.rotation;

        }
    }

    private IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(coolDownTime);
        canShoot = true;
    }

    private void determineDirection()
    {
        switch (directionMethod)
        {
            //Mouse
            case 1:
                Vector2 origin = transform.position;
                Vector2 target = GetMouseWorldPosition();
                Vector2 direction = target - origin;
                direction.Normalize();
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
            //PS4
            case 2:
                if (Input.GetAxis("Mouse X") > 0.01 || Input.GetAxis("Mouse Y") > 0.01 || Input.GetAxis("Mouse X") < -0.01 || Input.GetAxis("Mouse Y") < -0.01)
                    transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * Mathf.Rad2Deg, Vector3.forward);
                break;
            //XBOX
            case 3:
                break;   
        }
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
