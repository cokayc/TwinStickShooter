using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject bulletPrefab;
    public float coolDownTime;
    public float rotationSpeed;

    private Rigidbody2D myRB;
    private bool canShoot;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        myRB.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetAxis("Mouse X") > 0.01 || Input.GetAxis("Mouse Y") > 0.01|| Input.GetAxis("Mouse X") < -0.01 || Input.GetAxis("Mouse Y") < -0.01)
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * Mathf.Rad2Deg, Vector3.forward);
        if(Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine(ShotCooldown());
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }

    public IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(coolDownTime);
        canShoot = true;
    }
}
