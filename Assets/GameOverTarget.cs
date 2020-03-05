using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTarget : MonoBehaviour
{
    public GameObject button;

    private float movementThreshold;
    private Vector3 buttonPos;
    private Vector3 buttonSize;
    // Start is called before the first frame update
    void Start()
    {
        movementThreshold = 0.02f;
        Vector3 screenCenter = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, -250, 0) + screenCenter);
        buttonPos = transform.position;
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
