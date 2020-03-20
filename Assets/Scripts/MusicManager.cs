using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static float masterVolume;
    private static float musicVolume;
    private static float effectsVolume;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    private void Update()
    {
        var volumes = gm.GetVolumes();
        masterVolume = volumes.x;
        musicVolume = volumes.y;
        effectsVolume = volumes.z;
    }

    public void SetMasterVolume(float val)
    {
        gm = GameManager.instance;
        gm.SetMasterVolume(val);
        masterVolume = val;
    }

    public void SetEffectsVolume(float val)
    {
        gm = GameManager.instance;
        gm.SetEffectsVolume(val);
        effectsVolume = val;
    }

    public void SetMusicLevel(float val)
    {
        gm = GameManager.instance;
        gm.SetMusicLevel(val);
        musicVolume = val;
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public Vector3 GetVolumes()
    {
        return new Vector3(masterVolume, musicVolume, effectsVolume);
    }
}

