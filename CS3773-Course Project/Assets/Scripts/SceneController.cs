using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Opens the Register scene
    public void OpenRegister()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    // Opens the Login scene
    public void OpenLogin()
    {
        SceneManager.LoadScene("LoginScene");
    }

    // Temporary login button
    public void Login()
    {
        Debug.Log("Login button clicked.");
    }
}