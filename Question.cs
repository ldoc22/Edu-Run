using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubjectType { Math, English };
public enum QuestionDifficulty { Easy, Medium, Hard };

[CreateAssetMenu(fileName = "Questions", menuName = "ScriptableObjects/Question")]
public class Question : ScriptableObject
{

   
    public SubjectType Subject;
   
    public QuestionDifficulty questionDifficulty;

    public string QuestionText;

    public float QuestionTime;

    public string[] answerContext = new string[3];

    public int CorrectChoice;

    public Answer[] answers = new Answer[3];

    public void Init()
    {
        for (int i = 0; i < answers.Length; i++)
        {
            if((i+1) == CorrectChoice)
            {
                answers[i] = new Answer(answerContext[i], true);
            }
            else
            {
                answers[i] = new Answer(answerContext[i], false);
            }
        }
        
    }

    public void Create(string text, float time, string [] answers, int rightAnswer)
    {
        QuestionTime = time;
        answerContext = answers;
        CorrectChoice = rightAnswer;
        QuestionText = text;
    }
    


   
}
