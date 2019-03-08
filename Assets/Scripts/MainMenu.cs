using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject howTo;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowTo()
    {
        main.SetActive(false);
        howTo.SetActive(true);
    }

    public void Back()
    {
        howTo.SetActive(false);
        main.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
