using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject howTo;
    public GameObject movementTip;
    public GameObject cameraTip;
    public GameObject puzzlesTip;

    void Start()
    {
        Back();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowTo()
    {
        main.SetActive(false);
        howTo.SetActive(true);
        MovementTip();
    }

    public void Back()
    {
        howTo.SetActive(false);
        main.SetActive(true);
    }

    public void MovementTip()
    {
        movementTip.SetActive(true);
        cameraTip.SetActive(false);
        puzzlesTip.SetActive(false);
    }

    public void CameraTip()
    {
        movementTip.SetActive(false);
        cameraTip.SetActive(true);
        puzzlesTip.SetActive(false);
    }

    public void PuzzlesTip()
    {
        movementTip.SetActive(false);
        cameraTip.SetActive(false);
        puzzlesTip.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
