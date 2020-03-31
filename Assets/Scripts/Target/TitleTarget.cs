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
    public GameObject dummyButton;
    private Vector3 buttonSize;
    private Vector3 startButtonPos;
    private Vector3 tutorialButtonPos;
    private Vector3 creditsButtonPos;
    // Start is called before the first frame update
    protected override void Initialize()
    {
        buttonSize = Camera.main.ScreenToWorldPoint(new Vector3(350, 50, 0)+screenCenter);
        startButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 120, 0) + screenCenter);
        tutorialButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0) + screenCenter);
        creditsButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -120, 0) + screenCenter);
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
        else
            dummyButton.GetComponent<Button>().Select();

        if (Input.GetButton("Fire1")||Input.GetButton("Submit"))
        {
            if (FitsInBox(transform.position, startButtonPos, buttonSize))
                startButton.GetComponent<Button>().onClick.Invoke();
            else if (FitsInBox(transform.position, tutorialButtonPos, buttonSize))
                SceneManager.LoadScene("Controller Tutorial");
            else if (FitsInBox(transform.position, creditsButtonPos, buttonSize))
                creditsButton.GetComponent<Button>().onClick.Invoke();
        }

    }

    
}
