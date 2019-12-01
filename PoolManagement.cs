using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PoolManagement : MonoBehaviour
{


    [SerializeField]
    private Question [] QuestionPool;

    [SerializeField]
    private Object[] Objects;
    [SerializeField]
    private GameObject[] CharacterChoices = new GameObject[3];

    public static PoolManagement Pool;

    private void OnEnable()
    {
        if (PoolManagement.Pool == null)
        {
            PoolManagement.Pool = this;
        }
        else
        {
            if (PoolManagement.Pool != this)
            {
                Destroy(this.gameObject);
            }
        }
        
          
    }

    public Question [] GetQuestions(SubjectType sType, QuestionDifficulty qDiff)
    {
        GameStateStorage.instance.GameDifficulty = qDiff;
        GameStateStorage.instance.GameSubject = sType;
        List<Question> Selected = new List<Question>();
        foreach(Question q in QuestionPool)
        {
            if (q.Subject == sType && q.questionDifficulty == qDiff)
                Selected.Add(q);
        }
        return Selected.ToArray();
    }

    public GameObject [] GetCharacterChoices()
    {
        return CharacterChoices;
    }
}
