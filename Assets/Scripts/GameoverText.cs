using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "You Scored " + GameManager.instance.score + " Points";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
