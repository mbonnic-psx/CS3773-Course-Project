using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartManager : MonoBehaviour
{
    public TMP_Text MessageText;
    public TMP_Text SubtotalText;

    public CartItemData[] CartItems;

    public async void AddItem(int ProductId, int Quantity)
    {
        if (!UserSession.IsLoggedIn())
        {
            MessageText.text = "Please log in first.";
            return;
        }

        string CartUrl = ServerRequest.SERVER_URL + "/addCartItem.php";

        Dictionary<string, string> CartData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "productId", ProductId.ToString() },
            { "quantity", Quantity.ToString() }
        };

        string ResultText = await ServerRequest.SendPostRequest(CartUrl, CartData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        MessageText.text = Result.message;
    }

    public async void LoadCart()
    {
        string CartUrl = ServerRequest.SERVER_URL + "/getCart.php?userId=" + UserSession.UserId;
        string ResultText = await ServerRequest.SendGetRequest(CartUrl);
        CartResponse Result = JsonUtility.FromJson<CartResponse>(ResultText);

        MessageText.text = Result.message;

        if (Result.success)
        {
            CartItems = Result.cartItems;
            SubtotalText.text = "Subtotal: $" + Result.subtotal.ToString("0.00");

            foreach (CartItemData CartItem in CartItems)
            {
                Debug.Log(CartItem.itemName + " x" + CartItem.quantity +
                          " = $" + CartItem.lineTotal.ToString("0.00"));
            }
        }
    }

    public async void UpdateItem(int CartItemId, int Quantity)
    {
        string CartUrl = ServerRequest.SERVER_URL + "/updateCartItem.php";

        Dictionary<string, string> CartData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "cartItemId", CartItemId.ToString() },
            { "quantity", Quantity.ToString() }
        };

        string ResultText = await ServerRequest.SendPostRequest(CartUrl, CartData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        MessageText.text = Result.message;

        if (Result.success)
            LoadCart();
    }

    public async void DeleteItem(int CartItemId)
    {
        string CartUrl = ServerRequest.SERVER_URL + "/deleteCartItem.php";

        Dictionary<string, string> CartData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "cartItemId", CartItemId.ToString() }
        };

        string ResultText = await ServerRequest.SendPostRequest(CartUrl, CartData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        MessageText.text = Result.message;

        if (Result.success)
            LoadCart();
    }
}
