using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject WASD;
    public GameObject rightClick;
    public GameObject leftClick;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        rightClick.SetActive(false);
        leftClick.SetActive(false);
        WASD.SetActive(true);
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        bool moving = true;
        bool movedHorizontal = false;
        bool movedVertical = false;
        while (moving)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
                movedVertical = true;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                movedHorizontal = true;
            if(movedHorizontal&&movedVertical)
                moving = false;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        WASD.SetActive(false);
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        leftClick.SetActive(true);
        bool hasShot = false;
        while (!hasShot)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                hasShot = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        leftClick.SetActive(false);
        StartCoroutine(Possess());
    }

    public IEnumerator Possess()
    {
        Instantiate(enemy, PlayerController.instance.currentEnemy.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        GameObject startingEnemy = PlayerController.instance.currentEnemy.gameObject;
        rightClick.SetActive(true);
        bool hasPossessed = false;
        while (!hasPossessed)
        {
            if (!startingEnemy.Equals(PlayerController.instance.currentEnemy.gameObject))
                hasPossessed = true;
            yield return null;
        }
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Title");
    }

}
