using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
