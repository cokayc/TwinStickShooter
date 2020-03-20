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
        backgroundMusic = GetComponent<AudioSource>();

    }

    private void Start()
    {
        StartCoroutine(StartMusic());

        effectsLevel = 1;
        totalSoundLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        backgroundMusic.volume = totalSoundLevel * musicLevel;
    }


    public void LoadLevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
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

    public IEnumerator StartMusic()
    {
        musicLevel = 0;
        backgroundMusic.Play();
        while(musicLevel < 1)
        {
            musicLevel += 0.001f;
            yield return null;
        }
    }

    public void ToggleMusic()
    {
        if (backgroundMusic.isPlaying)
            backgroundMusic.Pause();
        else
            backgroundMusic.Play();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level One");
    }
}
