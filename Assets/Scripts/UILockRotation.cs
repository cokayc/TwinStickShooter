using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILockRotation : MonoBehaviour
{
    private Vector3 relativePosition;
    private Quaternion relativeRotation;
    void Start()
    {
        relativePosition = transform.localPosition;
        relativeRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = relativeRotation;
        transform.position = transform.parent.position + relativePosition;
    }
}
