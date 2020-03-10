using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTarget : Target
{
    public GameObject quitButton;
    public GameObject masterVolumeSlider;
    public GameObject musicVolumeSlider;
    public GameObject effectVolumeSlider;
    public GameObject dummyButton;

    private Vector3 quitButtonPos;
    private Vector3 quitButtonSize;
    private Vector3 sliderSize;
    private Vector3 masterSliderPos;
    private Vector3 musicSliderPos;
    private Vector3 effectSliderPos;
    // Start is called before the first frame update
    protected override void Initialize()
    {
        quitButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 50, Camera.main.pixelHeight - 50, 0));
        quitButtonSize = Camera.main.ScreenToWorldPoint(new Vector3(45, 45, 0) + screenCenter);
        sliderSize = Camera.main.ScreenToWorldPoint(new Vector3(280, 35, 0) + screenCenter);
        masterSliderPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 150, 0) + screenCenter);
        musicSliderPos = new Vector3(0, 0, 0);
        effectSliderPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -150, 0) + screenCenter);
    }

    // Update is called once per frame
    protected override void LookForButtons()
    {

        if (Target.FitsInBox(transform.position, quitButtonPos, quitButtonSize))
        {
            quitButton.GetComponent<Button>().Select();
            if (Input.GetButton("Fire1"))
            {
                quitButton.GetComponent<Button>().onClick.Invoke();
            }
            return;
        }
        else
            dummyButton.GetComponent<Button>().Select();
        if (Target.FitsInBox(transform.position, masterSliderPos, sliderSize) && Input.GetButton("Fire1"))
        {
            masterVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (masterSliderPos - sliderSize).x) / (2 * sliderSize.x);
        }
        else if (Target.FitsInBox(transform.position, musicSliderPos, sliderSize) && Input.GetButton("Fire1"))
        {
            musicVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (musicSliderPos - sliderSize).x) / (2 * sliderSize.x);
        }
        else if (Target.FitsInBox(transform.position, effectSliderPos, sliderSize) && Input.GetButton("Fire1"))
        {
            effectVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (effectSliderPos - sliderSize).x) / (2 * sliderSize.x);
        }



    }

}
