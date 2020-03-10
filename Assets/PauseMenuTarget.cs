﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuTarget : Target
{

    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject quitButton;
    public GameObject masterVolumeSlider;
    public GameObject musicVolumeSlider;
    public GameObject effectsVolumeSlider;
    private Vector3 buttonSize;
    private Vector3 sliderSize;
    private Vector3 resumeButtonPos;
    private Vector3 restartButtonPos;
    private Vector3 quitButtonPos;
    private Vector3 masterVolumePos;
    private Vector3 musicVolumePos;
    private Vector3 effectsVolumePos;


    protected override void Initialize()
    {
        buttonSize = Camera.main.ScreenToWorldPoint(new Vector3(80, 30, 0) + screenCenter);
        sliderSize = Camera.main.ScreenToWorldPoint(new Vector3(80, 10, 0) + screenCenter);
        resumeButtonPos = new Vector3(0, 2.5f, 0);
        restartButtonPos = new Vector3(0, 1.25f, 0);
        quitButtonPos = new Vector3(0, 0, 0);
        masterVolumePos = new Vector3(0, -1.5f, 0);
        musicVolumePos = new Vector3(0, -2.5f, 0);
        effectsVolumePos = new Vector3(0, -3.5f, 0);

    }

    protected override void LookForButtons()
    {
        if(FitsInBox(transform.position, resumeButtonPos, buttonSize))
        {
            resumeButton.GetComponent<Button>().Select();
        }
        else if(FitsInBox(transform.position, restartButtonPos, buttonSize))
        {
            restartButton.GetComponent<Button>().Select();
        }
        else if(FitsInBox(transform.position, quitButtonPos, buttonSize))
        {
            quitButton.GetComponent<Button>().Select();
        }

        if(Input.GetButton("Fire1"))
        {
            if (FitsInBox(transform.position, resumeButtonPos, buttonSize))
            {
                resumeButton.GetComponent<Button>().onClick.Invoke();
            }
            else if (FitsInBox(transform.position, restartButtonPos, buttonSize))
            {
                restartButton.GetComponent<Button>().onClick.Invoke();
            }
            else if (FitsInBox(transform.position, quitButtonPos, buttonSize))
            {
                quitButton.GetComponent<Button>().onClick.Invoke();
            }
            else if (FitsInBox(transform.position, masterVolumePos, sliderSize))
            {
                masterVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (masterVolumePos - sliderSize).x) / (2 * sliderSize.x);
            }
            else if (FitsInBox(transform.position, musicVolumePos, sliderSize))
            {
                musicVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (musicVolumePos - sliderSize).x) / (2 * sliderSize.x);
            }
            else if (FitsInBox(transform.position, effectsVolumePos, sliderSize))
            {
                effectsVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (effectsVolumePos - sliderSize).x) / (2 * sliderSize.x);
            }
        }
    }
}
