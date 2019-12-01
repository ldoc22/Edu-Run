using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Question []  Questions = new Question[3];
    public int NumOfQuestions;

    public float PlayerSpeed;

    public float SideMovementSpeed;
    [SerializeField]
    private int CurrentQuestionIndex;

    [SerializeField]
    private int CurrentRowIndex;

    private Timer timer;
    private UImanager uimanager;
    private MapCreation MC;

    private PlayerMovement PM;

    private AudioManager AM;

    public bool Spoof;

    private int Score;

    public int TimeAfterDeath;

    [SerializeField]
    private float questionTime;
    // Start is called before the first frame update
    void Awake()
    {
        Questions = GameStateStorage.instance.GetSelectedQuestions();

        switch (GameStateStorage.instance.GameDifficulty)
        {
            case QuestionDifficulty.Easy:
              questionTime = 5f;
                break;
            case QuestionDifficulty.Medium:
                questionTime = 7f;
                break;
            case QuestionDifficulty.Hard:
                questionTime = 10f;
                break;


        }
        PlayerSpeed = 30 / questionTime;

        for (int i = Questions.Length - 1;  i > 0  ; i--)
        {
            int j = Random.Range(0, i + 1);
            Question tmp = Questions[i];
            Questions[i] = Questions[j];
            Questions[j] = tmp;
        }
     
        
        AM = AudioManager.instance;
        PM = GetComponent<PlayerMovement>();
        uimanager = GetComponent<UImanager>();
        MC = GetComponent<MapCreation>();
        timer = GetComponent<Timer>();
        //Questions = GameStateStorage.instance.GetSelectedQuestions();
        //Questions = new Question[NumOfQuestions];
        for (int i = 0; i < Questions.Length; i++)
        {
            Questions[i].Init();
            for (int j = 0; j < Questions[i].answers.Length; j++)
            {
                print(Questions[i].answers[j].getContext() + "\t" + Questions[i].answers[j].isCorrect().ToString());
            }
        }
        CurrentQuestionIndex = 0;
        
        
    }

    private void OnEnable()
    {
        timer.setTargetTime(questionTime);
        timer.StartTime();
        uimanager.UpdateQuestionText(Questions[CurrentQuestionIndex].QuestionText);
        uimanager.StartAnswers(Questions[CurrentQuestionIndex].answerContext);
        uimanager.SetScore(0);
        //PlayerSpeed = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        uimanager.UpdateTimerText(timer.getTime());

        if (Spoof)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PM.SwipeLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PM.SwipeRight();
            }
        }

    }

    public void TimesUp()
    {
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if(Questions[CurrentQuestionIndex].CorrectChoice == CurrentRowIndex + 1)
        {
            uimanager.RightTextColor(true);
            AM.correct();
            MC.HighlightCheckZone(CurrentRowIndex);
            print("Correct");
            CurrentQuestionIndex++; 
            Score++;
            MC.NextZone();
        }
        else
        {
            uimanager.RightTextColor(false);
            AM.fall();
            print("Incorrect");
            MC.HighlightCheckZone(Questions[CurrentQuestionIndex].CorrectChoice -1);
            PM.Player.GetComponent<Rigidbody>().useGravity = true;
            StartCoroutine(LoadBackToMenuAfterDead(TimeAfterDeath));
        }
        uimanager.SetScore(Score);
    }

    public void NextQuestion()
    {
        timer.setTargetTime(questionTime);
        //timer.setTargetTime(Questions[CurrentQuestionIndex].QuestionTime);
        timer.StartTime();
        uimanager.UpdateQuestionText(Questions[CurrentQuestionIndex].QuestionText);
        uimanager.StartAnswers(Questions[CurrentQuestionIndex].answerContext);
        // PlayerSpeed = 30 / Questions[CurrentQuestionIndex].QuestionTime;
        //PlayerSpeed = 6f;
    }

    public float getSpeed()
    {
        return PlayerSpeed;
    }

    public void SetRowIndex(int index)
    {
        CurrentRowIndex = index;
    }
    IEnumerator LoadBackToMenuAfterDead(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayFabManager.PFM.EndGameScoreCheck(Score);
        EduSceneManager.SM.LoadScene(0);
    }
}
