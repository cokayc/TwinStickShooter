﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public GameObject musicManager;
    public int sliderNum;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        switch (sliderNum)
        {
            case 0: 
                slider.value = musicManager.GetComponent<MusicManager>().GetVolumes().x;
                break;
            case 1:
                slider.value = musicManager.GetComponent<MusicManager>().GetVolumes().y;
                break;
            case 2:
                slider.value = musicManager.GetComponent<MusicManager>().GetVolumes().z;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
