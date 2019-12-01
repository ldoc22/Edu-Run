using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource death;
    public AudioSource win;
    public AudioSource move;
    public AudioSource buttonPress;

    //Makes singleton
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void fall()
    {
        death.Play();
    }

    public void correct()
    {
        win.Play();
    }

    public void swipe()
    {
        move.Play();
    }

    public void click()
    {
        buttonPress.Play();
    }
}
