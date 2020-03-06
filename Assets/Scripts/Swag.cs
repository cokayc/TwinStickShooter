using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Swag : MonoBehaviour
{

    public GameObject image;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        Color color = image.GetComponent<Image>().color;
        color.a = 0;
        while(color.a <= 1)
        {
            image.GetComponent<Image>().color = color;
            text.GetComponent<Text>().color = color;
            yield return null;
            color.a += 0.01f;
        }
        yield return new WaitForSeconds(0.4f);
        while (color.a >= 0)
        {
            image.GetComponent<Image>().color = color;
            text.GetComponent<Text>().color = color;
            yield return null;
            color.a -= 0.01f;
        }
        SceneManager.LoadScene("Title");

    }
}
