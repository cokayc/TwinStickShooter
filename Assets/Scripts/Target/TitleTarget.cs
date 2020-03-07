using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleTarget : Target
{
    public GameObject startButton;
    public GameObject tutorialButton;
    public GameObject creditsButton;
    public GameObject soundButton;
    public GameObject dummyButton;
    private Vector3 buttonSize;
    private Vector3 soundButtonSize;
    private Vector3 startButtonPos;
    private Vector3 tutorialButtonPos;
    private Vector3 creditsButtonPos;
    private Vector3 soundButtonPos;
    // Start is called before the first frame update
    protected override void Initialize()
    {
        buttonSize = Camera.main.ScreenToWorldPoint(new Vector3(350, 50, 0)+screenCenter);
        soundButtonSize = Camera.main.ScreenToWorldPoint(new Vector3(50, 50, 0) + screenCenter);
        startButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 120, 0) + screenCenter);
        tutorialButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0) + screenCenter);
        creditsButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -120, 0) + screenCenter);
        soundButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -380, 0) + screenCenter);
}

    // Update is called once per frame
    protected override void LookForButtons()
    {
        
        if (FitsInBox(transform.position, startButtonPos, buttonSize))
            startButton.GetComponent<Button>().Select();
        else if (FitsInBox(transform.position, tutorialButtonPos, buttonSize))
            tutorialButton.GetComponent<Button>().Select();
        else if (FitsInBox(transform.position, creditsButtonPos, buttonSize))
            creditsButton.GetComponent<Button>().Select();
        else if (FitsInBox(transform.position, soundButtonPos, soundButtonSize))
            soundButton.GetComponent<Button>().Select();
        else
            dummyButton.GetComponent<Button>().Select();

        if (Input.GetButton("Fire1"))
        {
            if (FitsInBox(transform.position, startButtonPos, buttonSize))
                startButton.GetComponent<Button>().onClick.Invoke();
            else if (FitsInBox(transform.position, tutorialButtonPos, buttonSize))
                SceneManager.LoadScene("Controller Tutorial");
            else if (FitsInBox(transform.position, creditsButtonPos, buttonSize))
                creditsButton.GetComponent<Button>().onClick.Invoke();
            else if (FitsInBox(transform.position, soundButtonPos, soundButtonSize))
                soundButton.GetComponent<Button>().onClick.Invoke();
        }

    }

    
}
