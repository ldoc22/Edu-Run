using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EduSceneManager : MonoBehaviour
{

    public static EduSceneManager SM;
    public int currentSceneIndex;

    private void OnEnable()
    {
        if (EduSceneManager.SM == null)
        {
            EduSceneManager.SM = this;
        }
        else
        {
            if (EduSceneManager.SM != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void NextScene()
    {

        SceneManager.LoadScene(currentSceneIndex + 1);
        currentSceneIndex++;


    }
    public void LoadScene(int i)
    {
        currentSceneIndex = i;

        SceneManager.LoadScene(i);
    }
}
