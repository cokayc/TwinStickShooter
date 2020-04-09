using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public bool isPaused;
    private AudioSource backgroundMusic;
    private float effectsLevel;
    private float totalSoundLevel;
    private float musicLevel;
    public static GameManager instance;
    public Text timerText;
    public Text scoreText;

    [HideInInspector]
    public float timer;
    [HideInInspector]
    public int score;

    private float currentLevelStartTime;

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
        timer = 0;
        score = 0;
        currentLevelStartTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        backgroundMusic.volume = totalSoundLevel * musicLevel;
        timer += Time.deltaTime;
        timerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
        scoreText.text = "Score: " + score;
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
        while(musicLevel < 0.5)
        {
            musicLevel += 0.001f;
            yield return null;
        }
    }

    public void NextLevel()
    {
        float timeRemaining = 75 + currentLevelStartTime - timer;
        score += ((timeRemaining > 0) ? (int) timeRemaining : 0);
        score += 100;
        currentLevelStartTime = timer;
        SceneManager.LoadScene("Level One");
    }
}
