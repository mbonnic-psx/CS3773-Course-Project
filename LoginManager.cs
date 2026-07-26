using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField LoginInput;
    public TMP_InputField PasswordInput;
    public TMP_Text MessageText;

    public async void LoginUser()
    {
        string LoginName = LoginInput.text.Trim();
        string UserPassword = PasswordInput.text;

        if (LoginName == "" || UserPassword == "")
        {
            MessageText.text = "Enter your login and password.";
            return;
        }

        string LoginUserUrl = ServerRequest.SERVER_URL + "/loginUser.php";

        Dictionary<string, string> UserData = new Dictionary<string, string>()
        {
            { "loginName", LoginName },
            { "password", UserPassword }
        };

        string ResultText = await ServerRequest.SendPostRequest(LoginUserUrl, UserData);
        LoginResponse Result = JsonUtility.FromJson<LoginResponse>(ResultText);

        MessageText.text = Result.message;

        if (Result.success)
        {
            UserSession.UserId = Result.userId;
            UserSession.Email = Result.email;
            UserSession.Username = Result.username;
        }
    }

    public void LogoutUser()
    {
        UserSession.Logout();
        MessageText.text = "You are logged out.";
    }
}
