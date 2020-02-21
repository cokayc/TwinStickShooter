using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Sprite pauseImage;
    public Sprite playImage;
    private bool isPaused;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            image.sprite = pauseImage;
            Time.timeScale = 1;
        }
        else
        {
            image.sprite = playImage;
            Time.timeScale = 0;
        }

    }
}
