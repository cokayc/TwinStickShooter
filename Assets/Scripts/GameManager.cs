using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused;
    private AudioSource backgroundMusic;
    private float effectsLevel;
    private float totalSoundLevel;
    private float musicLevel;
    public static GameManager instance;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        effectsLevel = 1;
        musicLevel = 1;
        totalSoundLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        backgroundMusic.volume = totalSoundLevel * musicLevel;
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Level One");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void SoundSettings()
    {
        SceneManager.LoadScene("Sound Settings");
    }

    public void SetMasterVolume(float val)
    {
        totalSoundLevel=val;
    }

    public float GetEffectsLevel()
    {
        return effectsLevel * totalSoundLevel;
    }

    public void SetEffectsVolume(float val)
    {
        effectsLevel = val;
    }

    public void SetMusicLevel(float val)
    {
        musicLevel = val;
    }

    public Vector3 GetVolumes()
    {
        return new Vector3(totalSoundLevel, musicLevel, effectsLevel);
    }
}
