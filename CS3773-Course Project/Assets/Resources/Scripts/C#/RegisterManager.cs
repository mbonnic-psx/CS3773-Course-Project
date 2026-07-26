using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegisterManager : MonoBehaviour
{
    public UnityEngine.UI.InputField EmailInput;
    public UnityEngine.UI.InputField FirstNameInput;
    public UnityEngine.UI.InputField LastNameInput;
    public UnityEngine.UI.InputField PasswordInput;
    public UnityEngine.UI.InputField ConfirmPasswordInput;
    public TMP_Text MessageText;

    public async void RegisterUser()
    {
        if (EmailInput == null || FirstNameInput == null || LastNameInput == null || PasswordInput == null || ConfirmPasswordInput == null)
        {
            Debug.LogError("One or more register inputs are not referenced!");
            return;
        }

        string UserEmail = EmailInput.text.Trim();
        string FirstName = FirstNameInput.text.Trim();
        string LastName = LastNameInput.text.Trim();
        string UserPassword = PasswordInput.text;
        string ConfirmPassword = ConfirmPasswordInput.text;

        if (UserEmail == "" || FirstName == "" || LastName == "" || UserPassword == "" || ConfirmPassword == "")
        {
            if (MessageText != null) MessageText.text = "Please fill in every field.";
            return;
        }

        if (UserPassword != ConfirmPassword)
        {
            if (MessageText != null) MessageText.text = "Passwords do not match.";
            return;
        }

        string Username = FirstName + " " + LastName;

        string RegisterUserUrl = ServerRequest.SERVER_URL + "/registerUser.php";

        Dictionary<string, string> UserData = new Dictionary<string, string>()
        {
            { "email", UserEmail },
            { "username", Username },
            { "password", UserPassword }
        };

        if (MessageText != null) MessageText.text = "Creating account...";

        string ResultText = await ServerRequest.SendPostRequest(RegisterUserUrl, UserData);
        LoginResponse Result = JsonUtility.FromJson<LoginResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            UserSession.UserId = Result.userId;
            UserSession.Email = UserEmail;
            UserSession.Username = Username;
            UnityEngine.SceneManagement.SceneManager.LoadScene("ProductCatalogScene");
        }
    }
}
