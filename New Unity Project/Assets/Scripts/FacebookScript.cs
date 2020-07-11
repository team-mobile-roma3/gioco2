using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookScript : MonoBehaviour
{

    public Text FriendsText;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.LogError("Couldn't initialize");
            },
            isGameShown =>
            {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
            FB.ActivateApp();
    }

    #region Login / Logout
    public void FacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions);
    }

    public void FacebookLogout()
    {
        FB.LogOut();
    }
    #endregion

    public void FacebookShare()
    {
        FB.ShareLink(
             contentTitle: "I scored " + GameController.Score + " on Project C. Can you beat my score?",
         //    contentURL: new System.Uri("https://play.google.com/store/apps/details?id=com.flash.football"),
       //      photoURL: new System.Uri(" link di imgur"),
      //       contentDescription: "Try to click the ball to score a point. It is harder than it looks. Click to learn more.",
             callback: OnShare);
    }
    private void OnShare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
            Debug.Log("Share error: " + result.Error);
        else if (!string.IsNullOrEmpty(result.Error))
            Debug.Log(result.PostId);
        else
            Debug.Log("Success");
    }
    #region Inviting
    public void FacebookGameRequest()
    {
       FB.AppRequest("Hey! Come and play this awesome game!", title: "Project C");
    }

    
    #endregion

    public void GetFriendsPlayingThisGame()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            FriendsText.text = string.Empty;
            foreach (var dict in friendsList)
                FriendsText.text += ((Dictionary<string, object>)dict)["name"];
        });
    }
}