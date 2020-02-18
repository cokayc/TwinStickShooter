using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        gm.setMasterVolume(1);
        gm.setEffectsVolume(1);
        gm.setMusicLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMasterVolume(float val)
    {
        gm.setMasterVolume(val);
    }

    public void setEffectsVolume(float val)
    {
        gm.setEffectsVolume(val);
    }

    public void setMusicLevel(float val)
    {
        gm.setMusicLevel(val);
    }
}
