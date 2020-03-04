using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused;
    public AudioClip startSound;
    private AudioSource backgroundMusic;
    private float effectsLevel;
    private float totalSoundLevel;
    private float musicLevel;
    public static GameManager instance;
    private bool startMusicDone;
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
        StartCoroutine(StartMusic());
        effectsLevel = 1;
        musicLevel = 1;
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
        startMusicDone = false;
        backgroundMusic.PlayOneShot(startSound);
        yield return new WaitForSeconds(startSound.length-0.1f);
        startMusicDone = true;
        backgroundMusic.Play();
    }

    public void ToggleMusic()
    {
        if (backgroundMusic.isPlaying)
            backgroundMusic.Pause();
        else if(startMusicDone)
            backgroundMusic.Play();
    }
}
