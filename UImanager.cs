using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UImanager : MonoBehaviour
{

    public GameObject TimerTxt, ScoreTxt, QuestionTxt;
    public Text[] AnswersText;
    public GameObject[] textMeshAnswers = new GameObject[3];
    public GameObject[] oldAnswers = new GameObject[3];

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void UpdateTimerText(float time)
    {
        TimerTxt.GetComponent<TextMeshProUGUI>().SetText(time.ToString("N0"));
    }
    public void UpdateQuestionText(string words)
    {
        QuestionTxt.GetComponent<TextMeshProUGUI>().color = Color.white;
        QuestionTxt.GetComponent<TextMeshProUGUI>().SetText(words);
    }

    public void StartAnswers(string[] answers)
    {
        for (int i = 0; i < answers.Length; i++)
        {
            oldAnswers[i].GetComponent<TextMeshPro>().text = answers[i];

        }
    }

    public void SetScore(int score)
    {
        ScoreTxt.GetComponent<TextMeshProUGUI>().SetText(score.ToString());
    }

    public void RightTextColor(bool correct)
    {
        //foreach (GameObject text in oldAnswers) text.GetComponent<TextMeshPro>().faceColor = Color.red;
        //oldAnswers[correctIndex].GetComponent<TextMeshPro>().faceColor = Color.green;

        if (correct) QuestionTxt.GetComponent<TextMeshProUGUI>().color = Color.green;
        else QuestionTxt.GetComponent<TextMeshProUGUI>().color = Color.red;
    }
}