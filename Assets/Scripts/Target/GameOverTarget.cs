using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTarget : Target
{
    public GameObject button;
    public GameObject dummyButton;


    private Vector3 buttonPos;
    private Vector3 buttonSize;
    // Start is called before the first frame update
    protected override void Initialize()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, -200, 0) + screenCenter);
        buttonPos = transform.position;
        buttonSize = Camera.main.ScreenToWorldPoint(new Vector3(150, 50, 0) + screenCenter);
    }

    // Update is called once per frame
    protected override void LookForButtons()
    {
        if (Target.FitsInBox(transform.position, buttonPos, buttonSize))
        {
            button.GetComponent<Button>().Select();
            if (Input.GetButton("Fire1"))
            {
                button.GetComponent<Button>().onClick.Invoke();
            }
        }
        else
            dummyButton.GetComponent<Button>().Select();
    }

   
}
