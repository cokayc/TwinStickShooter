using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTarget : Target
{
    public GameObject button;


    private Vector3 buttonPos;
    private Vector3 buttonSize;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, -250, 0) + screenCenter);
        buttonPos = transform.position;
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
    }

   
}
