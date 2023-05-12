using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class GooglePlayServices : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("GooglePlayAutomaticLogin") == 1)
        {
            AuthenticateUser();
        }
    }

    public static void AuthenticateUser(int value = 1)
    {
        try
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    if (value == 2)
                    {
                        ShowLeaderboards();
                    }
                    else if(value == 3)
                    {
                        ShowAchievements();
                    }
                }
            });
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
        }
    }

    public static void ShowLeaderboards()
    {
        if (!Social.localUser.authenticated)
        {
            AuthenticateUser(2);
        }

        PostToLeaderboard();
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_score);
    }
    public static void ShowAchievements()
    {
        if (!Social.localUser.authenticated)
        {
            AuthenticateUser(3);
        }

        PostToAchievements();
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public static void PostToLeaderboard()
    {
        if (!Social.localUser.authenticated) { return; }

        Social.ReportScore(PlayerPrefs.GetInt("Record"), GPGSIds.leaderboard_score, (bool success) =>
        {
            if (success)
            {
                Debug.Log("SUCCESS");
            }
            else
            {
                Debug.Log(":<<<");
            }
        });
    }
    public static void PostToAchievements()
    {
        if (!Social.localUser.authenticated) { return; }

        if (PlayerPrefs.GetInt("Record") >= 25) {
            Social.ReportProgress(GPGSIds.achievement_newbies, 100f, success => { });
        }
        if (PlayerPrefs.GetInt("Record") >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_player, 100f, success => { });
        }
        if (PlayerPrefs.GetInt("Record") >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_professional, 100f, success => { });
        }
        if (PlayerPrefs.GetInt("Record") >= 200)
        {
            Social.ReportProgress(GPGSIds.achievement_hacker, 100f, success => { });
        }

        if (PlayerPrefs.GetInt("AchievementsCoins") >= 25)
        {
            Social.ReportProgress(GPGSIds.achievement_piggybank, 100f, success => { });
        }
        if (PlayerPrefs.GetInt("AchievementsCoins") >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_treasurer, 100f, success => { });
        }
        if (PlayerPrefs.GetInt("AchievementsCoins") >= 1000)
        {
            Social.ReportProgress(GPGSIds.achievement_millionaire, 100f, success => { });
        }

        if (PlayerPrefs.GetInt("AchievementsGifts") >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_santa_claus, 100f, success => { });
        }
    }
}
