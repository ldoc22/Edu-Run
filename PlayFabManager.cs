using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager PFM;

    public bool loggedIn;

    public string displayName;

    public int playerHighScore;
    public int tmpScore;

    private void OnEnable()
    {
        if (PlayFabManager.PFM ==  null)
        {
            PlayFabManager.PFM = this;
        }
        else
        {
            if(PlayFabManager.PFM != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void Start()
    {
        loggedIn = false;
        Login();
        playerHighScore = 10;
        

    }

    #region Login

    public void Login()
    {
       
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "144"; 
        }
        var request = new LoginWithCustomIDRequest { CustomId = "Logan", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        loggedIn = true;
        MainMenuUIManager.MMUI.UpdateMenuState(MenuState.Play);
        StartCloudUpdatePlayerStats();
     
        GetLeaderboard();
        
        

    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        loggedIn = false;
    }
    public void MobileLogin()
    {

#if UNITY_ANDROID
        var requestAndriod = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndriod, OnLoginMobileSuccess, OnLoginMobileFailure);

#endif
#if UNITY_IOS

        var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);

#endif

    }
   
    public void SetUserName()
    {
        
        
    }
    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        loggedIn = true;
        //PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = displayName },OnDisplayName, OnLoginMobileFailure );
        MainMenuUIManager.MMUI.UpdateMenuState(MenuState.Play);
        displayName = ReturnMobileID();

        StartCloudUpdatePlayerStats();
        
        GetLeaderboard();

    }
    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + "is your new display name");
    }

    public void GetDisplayName()
    {

        GetAccountInfoRequest request = new GetAccountInfoRequest();
        displayName = request.Username;
        PlayFabClientAPI.GetAccountInfo(request,OnGetUsernameResult, OnUserNameError);
       
    }
    
    public void OnGetUsernameResult(GetAccountInfoResult result)
    {
        displayName = result.AccountInfo.Username;
    }

    public void SetDisplayName()
    {

    }

    public void OnUserNameError(PlayFabError error)
    {
        MainMenuUIManager.MMUI.UpdateMenuState(MenuState.DisplayName);
    }



private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        loggedIn = false;
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    #endregion

   


    #region PlayerStats

    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "PlayerHighScore", Value = playerHighScore },
    }
        },
           result => { Debug.Log("User statistics updated"); },
           error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            if (eachStat.StatisticName.Equals("PlayerHighScore"))
            {
                playerHighScore = eachStat.Value;
            }

        }
        
    }
    public void EndGameScoreCheck(int score)
    {
        GetStats();
        if(playerHighScore <= score)
        {
            playerHighScore = score;
            StartCloudUpdatePlayerStats();
        }
    }

    // Build the request object and access the API
    public void StartCloudUpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new {PlayerHighScore = playerHighScore}, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnCloudHelloWorld, OnErrorShared);
    }
    // OnCloudHelloWorld defined in the next code block

    private static void OnCloudHelloWorld(ExecuteCloudScriptResult result)
    {
        // Cloud Script returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        //Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in Cloud Script
        Debug.Log((string)messageValue);
    }

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }


    #endregion Playerstats


    #region Leaderboard
    


    public void GetLeaderboard()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "PlayerHighScore", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }
    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        string tmp = "";
        foreach(PlayerLeaderboardEntry player in result.Leaderboard)
        {
            tmp = tmp + player.PlayFabId + "\t" + player.StatValue + "\n"; 
        }
        MainMenuUIManager.MMUI.SetLeaderboard(tmp);
    }

    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    #endregion Leaderboard

    #region Questions

    



    #endregion Questions
}



