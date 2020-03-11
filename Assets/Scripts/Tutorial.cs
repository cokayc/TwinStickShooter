using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject WASD;
    public GameObject rightStick;
    public GameObject rightClick;
    public GameObject leftClick;
    public GameObject enemy1;
    public GameObject enemy2;


    // Start is called before the first frame update
    void Start()
    {
        rightClick.SetActive(false);
        leftClick.SetActive(false);
        WASD.SetActive(true);
        if (rightStick != null)
            rightStick.SetActive(false);
        StartCoroutine(Move());
    }

    void Update()
    {

        if (PlayerController.instance.currentEnemy.GetComponentInChildren<Health>().healthPercent<0.5)
            StartCoroutine(Heal());
    }

    public IEnumerator Heal()
    {
        yield return new WaitForSeconds(0.3f);
        PlayerController.instance.currentEnemy.GetComponentInChildren<Health>().Restore(1);
    }

    public IEnumerator Move()
    {
        bool moving = true;
        bool movedHorizontal = false;
        bool movedVertical = false;
        Vector3 startPosition = PlayerController.instance.currentEnemy.gameObject.transform.position;
        while (moving)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Mathf.Abs((PlayerController.instance.currentEnemy.gameObject.transform.position - startPosition).y) > 2)
                movedVertical = true;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Mathf.Abs((PlayerController.instance.currentEnemy.gameObject.transform.position - startPosition).x) > 2)
                movedHorizontal = true;
            if(movedHorizontal&&movedVertical)
                moving = false;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        WASD.SetActive(false);
        if (PlayerController.instance.directionMethod == 2)
        {
            StartCoroutine(Spin());
        }
        else
        {
            StartCoroutine(Shoot());
        }
    }

    public IEnumerator Spin()
    {
        rightStick.SetActive(true);
        bool spun = false;
        List<Vector3> rotations = new List<Vector3>();
        while(!spun)
        {
            if(!rotations.Contains(PlayerController.instance.currentEnemy.gameObject.transform.rotation.eulerAngles))
                rotations.Add(PlayerController.instance.currentEnemy.gameObject.transform.rotation.eulerAngles);
            Debug.Log(rotations.Count);
            if (rotations.Count > 50)
                spun = true;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        rightStick.SetActive(false);
        StartCoroutine(Shoot());
    }
    public IEnumerator Shoot()
    {
        GameObject weakEnemy = Instantiate(enemy1, new Vector3(-15, 0, 0), Quaternion.identity);
        leftClick.SetActive(true);
        while (weakEnemy != null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2);
        leftClick.SetActive(false);
        StartCoroutine(Possess());
    }

    public IEnumerator Possess()
    {
        var bigBadEnemy = Instantiate(enemy2, new Vector3(15, 0, 0), Quaternion.identity);
        GameObject startingEnemy = PlayerController.instance.currentEnemy.gameObject;
        rightClick.SetActive(true);
        bool hasPossessed = false;
        while (!hasPossessed)
        {
            if (!startingEnemy.Equals(PlayerController.instance.currentEnemy.gameObject))
                hasPossessed = true;
            yield return null;
            if(bigBadEnemy == null)
                bigBadEnemy = Instantiate(enemy2, new Vector3(15, 0, 0), Quaternion.identity);
        }
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Title");
    }

}
