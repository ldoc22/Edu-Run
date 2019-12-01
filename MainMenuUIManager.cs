using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState { Login, Play, SubjectSelect, QuestionDifficultySelect, Create, Store, CharacterSelect, Leaderboard, DisplayName };

public class MainMenuUIManager : MonoBehaviour
{
    
    public MenuState currentMenuState;
    public GameObject LoginUI;
    public GameObject MainMenuUI;
    public GameObject SelectSubjectUI;
    public GameObject SelectQuestionDifficultyUI;
    public GameObject BackButton;
    public GameObject Leaderboard;
    public GameObject DisplayNameUI;

    public InputField displayNametext;

    private AudioManager AM;
    

    public GameObject CharacterSelectUI;

    public static MainMenuUIManager MMUI;

    public GameObject StorePanel;

    public QuestionDifficulty SelectedQuestionDifficulty;
    public SubjectType SelectedSubject;


    //Character Select Variables

    private GameObject[] CharacterSelectObjects;
    private GameObject[] CharacterChoices;
    private int CurrentCharacter;


    private void OnEnable()
    {
        if (MainMenuUIManager.MMUI == null)
        {
            MainMenuUIManager.MMUI = this;
        }
        else
        {
            if (MainMenuUIManager.MMUI != this)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        AM = AudioManager.instance;

        print(Screen.width);
        if (!PlayFabManager.PFM.loggedIn)

            UpdateMenuState(MenuState.Login);
        else
        {
            //PlayFabManager.PFM.GetDisplayName();
           // if (PlayFabManager.PFM.displayName.Equals("") || PlayFabManager.PFM.displayName.Equals(" ")  || PlayFabManager.PFM.displayName == null)
            //    UpdateMenuState(MenuState.DisplayName);
           // else
                UpdateMenuState(MenuState.Play);
        }

        CreateStore();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendPlay()
    {
        EduSceneManager.SM.NextScene();
    }
    public void SendLogin()
    {
        PlayFabManager.PFM.MobileLogin();
    }
    public void OnPlayClick()
    {
        
        UpdateMenuState(MenuState.SubjectSelect);
    }
    public void OnSubjectSelect()
    {
        
        UpdateMenuState(MenuState.QuestionDifficultySelect);
    }

    public void OnLeaderboard()
    {
        PlayFabManager.PFM.GetLeaderboard();
        UpdateMenuState(MenuState.Leaderboard);
        AM.click();
    }

    public void SetLeaderboard(string str)
    {
        Leaderboard.GetComponentInChildren<Text>().text = str;
    }


    #region CharcterSelect

    public void MoveRightButtonPress()
    {
        CharacterSelectObjects[CurrentCharacter].SetActive(false);
        if (CurrentCharacter >= CharacterSelectObjects.Length - 1)
            CurrentCharacter = 0;
        else
            CurrentCharacter++;
        CharacterSelectObjects[CurrentCharacter].SetActive(true);
        AM.click();
    }

    public void MoveLeftButtonPress()
    {
        CharacterSelectObjects[CurrentCharacter].SetActive(false);
        if (CurrentCharacter <= 0)
            CurrentCharacter = CharacterSelectObjects.Length - 1;
        else
            CurrentCharacter--;
        CharacterSelectObjects[CurrentCharacter].SetActive(true);
        AM.click();
    }

    public void SelectCharacter()
    {
        foreach (GameObject c in CharacterSelectObjects)
        {
            Destroy(c);
        }
        GameStateStorage.instance.StoreSelectedCharacter(PoolManagement.Pool.GetCharacterChoices()[CurrentCharacter]);
        OnPlayClick();
        AM.click();
    }

    public void ActivateCharacterSelect()
    {
        UpdateMenuState(MenuState.CharacterSelect);
        
        CreateCharacters();
        AM.click();
    }

    public void GoBack()
    {
        switch (currentMenuState)
        {
            case MenuState.SubjectSelect:
                currentMenuState = MenuState.CharacterSelect;
                break;
            case MenuState.QuestionDifficultySelect:
                currentMenuState = MenuState.SubjectSelect;
                break;
            case MenuState.CharacterSelect:
                foreach (GameObject c in CharacterSelectObjects)
                {
                    Destroy(c);
                }
                currentMenuState = MenuState.Play;
                break;
            case MenuState.Leaderboard:
                currentMenuState = MenuState.Play;
                break;
        }
        UpdateMenuState(currentMenuState);
        AM.click();

    }

    private void CreateCharacters()
    {
        CharacterChoices = PoolManagement.Pool.GetCharacterChoices();
        CharacterSelectObjects = new GameObject[CharacterChoices.Length];
        CurrentCharacter = 0;
        for (int i = 0; i < CharacterChoices.Length; i++)
        {

        
           GameObject c = Instantiate(CharacterChoices[i], transform.position, Quaternion.EulerRotation(180, 0, 0));
            c.SetActive(false);
            CharacterSelectObjects[i] = c;



        }
        CharacterSelectObjects[CurrentCharacter].SetActive(true);    
    }

    #endregion CharacterSelect


    #region GameSubjectChoices

    public void SetSubjectMath()
    {
        SelectedSubject = SubjectType.Math;
        AM.click();
    }

    public void SetSubjectEnglish()
    {
        SelectedSubject = SubjectType.English;
        AM.click();
    }

    public void SetDifficulty(int i)
    {
        switch (i)
        {
            case 0:
                SelectedQuestionDifficulty = QuestionDifficulty.Easy;
                break;
            case 1:
                SelectedQuestionDifficulty = QuestionDifficulty.Medium;
                break;
            case 2:
                SelectedQuestionDifficulty = QuestionDifficulty.Hard;
                break;

        }
        GameStateStorage.instance.StoreSelectedQuestions(PoolManagement.Pool.GetQuestions(SelectedSubject, SelectedQuestionDifficulty));
        AM.click();

    }


    #endregion GameSubjectChoices

    #region ChangeMenuState
    public void UpdateMenuState(MenuState MS)
    {
        TurnOffUIElements();
        currentMenuState = MS;
        switch (currentMenuState)
        {
            case MenuState.Login:
                LoginUI.SetActive(true);
                break;
            case MenuState.Play:
                MainMenuUI.SetActive(true);
                break;
            case MenuState.Store:
                UpdateUICurrency();
                StorePanel.SetActive(true);
                break;
            case MenuState.SubjectSelect:
                SelectSubjectUI.SetActive(true);
                BackButton.SetActive(true);
                break;
            case MenuState.QuestionDifficultySelect:
                SelectQuestionDifficultyUI.SetActive(true);
                BackButton.SetActive(true);
                break;
            case MenuState.CharacterSelect:
                CharacterSelectUI.SetActive(true);
                BackButton.SetActive(true);
                break;
            case MenuState.Leaderboard:
                Leaderboard.SetActive(true);
                BackButton.SetActive(true);
                
                break;
            case MenuState.DisplayName:
                DisplayNameUI.SetActive(true);
                break;
        }
    }

    private void TurnOffUIElements()
    {
        LoginUI.SetActive(false);
        MainMenuUI.SetActive(false);
        SelectQuestionDifficultyUI.SetActive(false);
        SelectSubjectUI.SetActive(false);
        CharacterSelectUI.SetActive(false);
        BackButton.SetActive(false);
        Leaderboard.SetActive(false);
        DisplayNameUI.SetActive(false);
    }

    #endregion ChangeMenuState

    #region Store
    public void ActivateStore()
    {
        
    }

    private void UpdateUICurrency()
    {
        //api call to user stats for currency
    }


    private void CreateStore()
    {
       // RectTransform SPRT = StorePanel.GetComponent<RectTransform>();
       
    }

    #endregion Store


    #region UpdateDisplayName

    public void SubmitDisplayName()
    {
        PlayFabManager.PFM.displayName = displayNametext.text;
        PlayFabManager.PFM.SetDisplayName();
        
    }

    #endregion UpdateDisplayName
}
