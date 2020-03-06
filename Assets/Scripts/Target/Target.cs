using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{

    protected static float movementThreshold = 0.02f;
    protected Vector3 screenCenter;

    private void Awake()
    {
        screenCenter = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
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
        LookForButtons();
    }

    public static bool FitsInBox(Vector3 pos, Vector3 boxPos, Vector3 boxSize)
    {
        Vector3 lowerCorner = boxPos - boxSize;
        Vector3 upperCorner = boxPos + boxSize;
        return pos.x > lowerCorner.x && pos.y > lowerCorner.y && pos.x < upperCorner.x && pos.y < upperCorner.y;
    }

    protected abstract void LookForButtons();
}
