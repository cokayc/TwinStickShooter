using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    private float movementThreshold;
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
    void Start()
    {
        movementThreshold = 0.02f;
        Vector3 screenCenter = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        buttonSize = Camera.main.ScreenToWorldPoint(new Vector3(350, 50, 0)+screenCenter);
        soundButtonSize = Camera.main.ScreenToWorldPoint(new Vector3(50, 50, 0) + screenCenter);
        startButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 120, 0) + screenCenter);
        tutorialButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0) + screenCenter);
        creditsButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -120, 0) + screenCenter);
        soundButtonPos = Camera.main.ScreenToWorldPoint(new Vector3(0, -380, 0) + screenCenter);

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
                tutorialButton.GetComponent<Button>().onClick.Invoke();
            else if (FitsInBox(transform.position, creditsButtonPos, buttonSize))
                creditsButton.GetComponent<Button>().onClick.Invoke();
            else if (FitsInBox(transform.position, soundButtonPos, soundButtonSize))
                soundButton.GetComponent<Button>().onClick.Invoke();
        }

    }

    public static bool FitsInBox(Vector3 pos, Vector3 boxPos, Vector3 boxSize)
    {
        Vector3 lowerCorner = boxPos - boxSize;
        Vector3 upperCorner = boxPos + boxSize;
        return pos.x > lowerCorner.x && pos.y > lowerCorner.y && pos.x < upperCorner.x && pos.y < upperCorner.y;
    }
}
