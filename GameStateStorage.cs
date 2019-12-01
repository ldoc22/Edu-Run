using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateStorage : MonoBehaviour
{

    public static GameStateStorage instance;

    private void OnEnable()
    {
        if (GameStateStorage.instance == null)
        {
            GameStateStorage.instance = this;
        }
        else
        {
            if (GameStateStorage.instance != this)
            {
                Destroy(this.gameObject);
            }

        }
    }

    [SerializeField]
    private Question[] SelectedQuestions;
    private GameObject SelectedCharacter;

    public SubjectType GameSubject;
    public QuestionDifficulty GameDifficulty;
    

    public void StoreSelectedQuestions(Question [] questions)
    {
        SelectedQuestions = questions;
    }

    public Question [] GetSelectedQuestions()
    {
        return SelectedQuestions;
    }

    public void StoreSelectedCharacter(GameObject character)
    {
        SelectedCharacter = character;
    }
    public GameObject GetSelectedCharacter()
    {
        return SelectedCharacter;
    }

   
}
