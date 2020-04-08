using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject target;
    public GameObject meat;
    public GameObject musicManager;
    public GameObject pauseButton;
    public GameObject redFlash;
    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.isPaused)
            gameObject.SetActive(false);
        target.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        if (Input.GetButtonDown("Jump"))
            pauseButton.GetComponent<PauseButton>().Resume();
    }

    public void Activate()
    {
        
        target.SetActive(false);
        meat.SetActive(false);
        redFlash.SetActive(false);
        StartCoroutine(Activation(true));
    }

    public void Deactivate()
    {
        meat.SetActive(false);
        target.SetActive(false);
        StartCoroutine(Activation(false));
    }

    public IEnumerator Activation(bool activate)
    {
        var size = menu.transform.localScale;
        for(float i = 0; i<=1; i+=0.1f)
        {
            if(activate)
            {
                size.x = i;
                menu.transform.localScale = size;
            }
            else
            {
                i += 0.1f;
                size.x = 1 - i;
                menu.transform.localScale = size;
            }
            yield return null;
        }
        if (!activate)
        {
            gameObject.SetActive(false);
        }
        else
        {
            meat.SetActive(true);
            if (PlayerController.instance.directionMethod == 1)
                target.SetActive(false);
            else
            {
                target.SetActive(true);
                target.transform.localPosition = new Vector3(0, 2.5f, 0);
            }
        }
    }

    public void Restart()
    {
        pauseButton.GetComponent<PauseButton>().Resume();
        Deactivate();
        PlayerController.instance.instantiated = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Title()
    {
        pauseButton.GetComponent<PauseButton>().Resume();
        Deactivate();
        SceneManager.LoadScene("Title");
    }
        
}
