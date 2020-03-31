using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Target : MonoBehaviour
{
    public static float movementThreshold = 0.02f;
    protected Vector3 screenCenter;

    private void Start()
    {
        screenCenter = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        if (PlayerController.instance.directionMethod == 1)
            gameObject.SetActive(false);
        Initialize();
    }


    // Update is called once per frame
    public void Update()
    { 
        Vector3 direction;
        float mouseX = Input.GetAxis("Horizontal");
        float mouseY = Input.GetAxis("Vertical");
        if (Mathf.Abs(mouseX) > movementThreshold || Mathf.Abs(mouseY) > movementThreshold)
        {

            direction = new Vector3(mouseX, mouseY, 0);
            direction.Normalize();
            //direction = Vector2.Perpendicular(direction);
            transform.localPosition += direction * 5 * Time.deltaTime;
        }
        LookForButtons();
    }

    public static bool FitsInBox(Vector3 pos, Vector3 boxPos, Vector3 boxSize)
    {
        Vector3 lowerCorner = boxPos - boxSize;
        Vector3 upperCorner = boxPos + boxSize;
        return pos.x > lowerCorner.x && pos.y > lowerCorner.y && pos.x < upperCorner.x && pos.y < upperCorner.y;
    }

    protected abstract void LookForButtons();
    protected abstract void Initialize();
}
