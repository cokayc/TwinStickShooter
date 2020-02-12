using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRB;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        myRB.velocity = speed * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
