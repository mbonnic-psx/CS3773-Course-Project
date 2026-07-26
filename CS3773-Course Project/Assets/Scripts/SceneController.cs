using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit Application");
        }
    }

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

    // Opens the Product Catalog scene
    public void OpenProductCatalog()
    {
        SceneManager.LoadScene("ProductCatalogScene");
    }

    // Opens the Shopping Cart scene
    public void OpenShoppingCart()
    {
        SceneManager.LoadScene("ShoppingCartScene");
    }

    // Opens the Address Management scene
    public void OpenAddressManagement()
    {
        SceneManager.LoadScene("AddressManagementScene");
    }

    // Opens the Checkout scene
    public void OpenCheckout()
    {
        SceneManager.LoadScene("CheckoutScene");
    }

    // Opens the Order History scene
    public void OpenOrderHistory()
    {
        SceneManager.LoadScene("OrderHistoryScene");
    }

    // Temporary login button
    public void Login()
    {
        Debug.Log("Login button clicked.");
    }
}