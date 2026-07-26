using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField EmailInput;
    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;
    public TMP_Text MessageText;

    public async void RegisterUser()
    {
        string UserEmail = EmailInput.text.Trim();
        string Username = UsernameInput.text.Trim();
        string UserPassword = PasswordInput.text;

        if (UserEmail == "" || Username == "" || UserPassword == "")
        {
            MessageText.text = "Please fill in every field.";
            return;
        }

        string RegisterUserUrl = ServerRequest.SERVER_URL + "/registerUser.php";

        Dictionary<string, string> UserData = new Dictionary<string, string>()
        {
            { "email", UserEmail },
            { "username", Username },
            { "password", UserPassword }
        };

        MessageText.text = "Creating account...";

        string ResultText = await ServerRequest.SendPostRequest(RegisterUserUrl, UserData);
        LoginResponse Result = JsonUtility.FromJson<LoginResponse>(ResultText);

        MessageText.text = Result.message;

        if (Result.success)
        {
            UserSession.UserId = Result.userId;
            UserSession.Email = UserEmail;
            UserSession.Username = Username;
        }
    }
}
