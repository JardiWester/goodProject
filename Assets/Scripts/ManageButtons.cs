using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public List<GameObject> thingsToTurnOn = new List<GameObject>();


    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        EnterPauseMenu();
    //    }
    //}

    private void Start()
    {
        foreach (GameObject obj in thingsToTurnOn)
        {
            obj.SetActive(false);
        }
    }

    public void EnterPauseMenu()
    {
        pauseMenu.SetActive(true);
        foreach (GameObject obj in thingsToTurnOn)
        {
            obj.SetActive(false);
        }
    }
    public void PlayGame()
    {
        pauseMenu.SetActive(false);
        foreach (GameObject obj in thingsToTurnOn)
        {
            obj.SetActive(true);
        }
    }

    public void ToggleOptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ExitOptionsMenu()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    
}
