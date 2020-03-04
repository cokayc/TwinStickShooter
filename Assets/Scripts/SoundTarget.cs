using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTarget : MonoBehaviour
{
    public GameObject quitButton;
    public GameObject masterVolumeSlider;
    public GameObject musicVolumeSlider;
    public GameObject effectVolumeSlider;

    private float movementThreshold;
    private Vector3 quitButtonPos;
    private Vector3 quitButtonSize;
    private Vector3 sliderSize;
    private Vector3 masterSliderPos;
    private Vector3 musicSliderPos;
    private Vector3 effectSliderPos;
    // Start is called before the first frame update
    void Start()
    {
        movementThreshold = 0.02f;
        Vector3 screenCenter = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        quitButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth - 50, Camera.main.pixelHeight - 50, 0));
        quitButtonSize = Camera.main.ScreenToWorldPoint(new Vector3(45, 45, 0) + screenCenter);
        sliderSize = Camera.main.ScreenToWorldPoint(new Vector3(280, 35, 0) + screenCenter);
        masterSliderPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 150, 0) + screenCenter);
        musicSliderPos = new Vector3(0, 0, 0);
        effectSliderPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -150, 0) + screenCenter);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.directionMethod == 1)
            gameObject.SetActive(false);
        Vector3 direction;
        float mouseX = Input.GetAxis("Horizontal");
        float mouseY = Input.GetAxis("Vertical");
        if (Mathf.Abs(mouseX) > movementThreshold || Mathf.Abs(mouseY) > movementThreshold)
        {
            direction = new Vector3(mouseX, mouseY, 0);
            direction.Normalize();
            //direction = Vector2.Perpendicular(direction);
            transform.position += direction * 5 * Time.deltaTime;
        }
        //Debug.Log(transform.position);
        if (FitsInBox(transform.position, quitButtonPos, quitButtonSize))
        {
            Debug.Log("we did it");
            quitButton.GetComponent<Button>().Select();
            if (Input.GetButton("Fire1"))
            {
                quitButton.GetComponent<Button>().onClick.Invoke();
            }
        }
        if (FitsInBox(transform.position, masterSliderPos, sliderSize) && Input.GetButton("Fire1"))
        {
            masterVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (masterSliderPos - sliderSize).x) / (2 * sliderSize.x);
        }
        else if (FitsInBox(transform.position, musicSliderPos, sliderSize) && Input.GetButton("Fire1"))
        {
            musicVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (musicSliderPos - sliderSize).x) / (2 * sliderSize.x);
        }
        else if (FitsInBox(transform.position, effectSliderPos, sliderSize) && Input.GetButton("Fire1"))
        {
            effectVolumeSlider.GetComponent<Slider>().value = (transform.position.x - (effectSliderPos - sliderSize).x) / (2 * sliderSize.x);
        }



    }

    private bool FitsInBox(Vector3 pos, Vector3 boxPos, Vector3 boxSize)
    {
        Vector3 lowerCorner = boxPos - boxSize;
        Vector3 upperCorner = boxPos + boxSize;
        return pos.x > lowerCorner.x && pos.y > lowerCorner.y && pos.x < upperCorner.x && pos.y < upperCorner.y;
    }
}
