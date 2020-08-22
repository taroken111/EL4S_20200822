using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public Fade fade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fade.FadeIn(2.0f,"SampleScene");
        }
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
