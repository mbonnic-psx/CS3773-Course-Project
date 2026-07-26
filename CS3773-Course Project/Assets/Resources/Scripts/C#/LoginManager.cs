using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public UnityEngine.UI.InputField LoginInput;
    public UnityEngine.UI.InputField PasswordInput;
    public TMP_Text MessageText;

    public async void LoginUser()
    {
        if (LoginInput == null || PasswordInput == null)
        {
            Debug.LogError("LoginInput or PasswordInput is not referenced!");
            return;
        }

        string LoginName = LoginInput.text.Trim();
        string UserPassword = PasswordInput.text;

        if (LoginName == "" || UserPassword == "")
        {
            if (MessageText != null) MessageText.text = "Enter your login and password.";
            return;
        }

        if (MessageText != null) MessageText.text = "Logging in...";

        string LoginUserUrl = ServerRequest.SERVER_URL + "/loginUser.php";

        Dictionary<string, string> UserData = new Dictionary<string, string>()
        {
            { "loginName", LoginName },
            { "password", UserPassword }
        };

        string ResultText = await ServerRequest.SendPostRequest(LoginUserUrl, UserData);
        LoginResponse Result = JsonUtility.FromJson<LoginResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            UserSession.UserId = Result.userId;
            UserSession.Email = Result.email;
            UserSession.Username = Result.username;
            UnityEngine.SceneManagement.SceneManager.LoadScene("ProductCatalogScene");
        }
    }

    public void LogoutUser()
    {
        UserSession.Logout();
        if (MessageText != null) MessageText.text = "You are logged out.";
    }
}
