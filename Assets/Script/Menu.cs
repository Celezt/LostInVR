using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    bool restart = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ReloadScene()
    {
        restart = true;
        gameObject.SetActive(false);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ShowCredits()
    {
        //Show credits instead of buttons
    }

    public void HideCredits()
    {
        //Show buttons again
    }

    public void Quit()
    {
        Application.Quit();
    }
}
