﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Sprite pauseImage;
    public Sprite playImage;
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
        gm = GameManager.instance;
        gm.isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        gm.ToggleMusic();
        if (isPaused)
        {
            image.sprite = pauseImage;
            Time.timeScale = 1;
            isPaused = false;
            gm.isPaused = false;
            pauseMenu.GetComponent<PauseMenu>().Deactivate();
        }
        else
        {
            image.sprite = playImage;
            Time.timeScale = 0;
            isPaused = true;
            gm.isPaused = true;
            pauseMenu.SetActive(true);
            pauseMenu.GetComponent<PauseMenu>().Activate();
        }

    }
}
