using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
}
