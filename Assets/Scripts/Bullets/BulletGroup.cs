using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  BulletGroup : MonoBehaviour
{
    public float[] angleDiff;
    public GameObject[] bullets;

    [HideInInspector]
    public Vector3 direction;

    private GameObject shooter;

    private void Start()
    {
       
        for  (int i = 0; i < bullets.Length; i++)
        {
            Vector3 dir = Quaternion.Euler(0, 0, angleDiff[i]) * direction;
            
            // Instantiate bullet and get reference to script
            var bullet = Instantiate(bullets[i], transform.position, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x) + 90)).GetComponent<Bullet>();
            bullet.SetShooter(shooter);
            bullet.direction = dir;
        }
        Destroy(gameObject);
    }

    public void SetShooter(GameObject s)
    {
        shooter = s;
    }




}
