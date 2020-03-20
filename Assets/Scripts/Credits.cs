using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public int scrollSpeed;
    public int maxHeight;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Scroll());
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public IEnumerator Scroll()
    {
        yield return new WaitForSeconds(3);
        while (transform.localPosition.y < maxHeight)
        {
            transform.localPosition += scrollSpeed * Vector3.up;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Title");
    }
}
