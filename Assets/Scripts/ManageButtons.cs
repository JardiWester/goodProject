using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageButtons : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnterPauseMenu();
        }
    }
    
    public void EnterPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    public void PlayGame()
    {
        //pauseMenu.SetActive(false);
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
