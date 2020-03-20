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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        target.transform.position = new Vector3(0, 2.5f, 0);
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
                target.SetActive(true);
        }
    }

    public void Restart()
    {
        pauseButton.GetComponent<PauseButton>().TogglePause();
        Deactivate();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Title()
    {
        pauseButton.GetComponent<PauseButton>().TogglePause();
        Deactivate();
        SceneManager.LoadScene("Title");
    }
        
}
