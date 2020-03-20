using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SliderController : MonoBehaviour
{
    public GameObject musicManager;
    public int sliderNum;

    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>().gameObject;
        slider = GetComponent<Slider>();
        
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}
