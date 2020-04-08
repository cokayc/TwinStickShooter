using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Sprite pauseImage;
    public Sprite playImage;
    public Sprite triangle;
    public GameObject pauseMenu;

    private bool isPaused;
    private Image image;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        image = GetComponent<Image>();
        if (PlayerController.instance.directionMethod == 2)
            image.sprite = triangle;
        gm = GameManager.instance;
        gm.isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump")&&!isPaused)
        {
            Pause();
        }
    }
    

    public void Pause()
    {
        image.sprite = playImage;
        Time.timeScale = 0;
        isPaused = true;
        gm.isPaused = true;
        pauseMenu.SetActive(true);
        pauseMenu.GetComponent<PauseMenu>().Activate();
        CameraControl.instance.screenShakeStrength = 0;
        CameraControl.instance.screenShakeTimer = 0;
        if (PlayerController.instance.directionMethod == 2)
            image.sprite = triangle;
    }

    public void Resume()
    {
        image.sprite = pauseImage;
        Time.timeScale = 1;
        isPaused = false;
        gm.isPaused = false;
        pauseMenu.GetComponent<PauseMenu>().Deactivate();
        if (PlayerController.instance.directionMethod == 2)
            image.sprite = triangle;
    }
}
