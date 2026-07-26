using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartManager : MonoBehaviour
{
    public TMP_Text MessageText;
    public TMP_Text SubtotalText;
    public Transform CartContainer;

    public CartItemData[] CartItems;

    private void Start()
    {
        // Set up layout on CartContainer dynamically if needed
        if (CartContainer != null)
        {
            var vlg = CartContainer.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
            if (vlg == null) vlg = CartContainer.gameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.spacing = 5;
            vlg.padding = new RectOffset(5, 5, 5, 5);

            var csf = CartContainer.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            if (csf == null) csf = CartContainer.gameObject.AddComponent<UnityEngine.UI.ContentSizeFitter>();
            csf.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
        }

        LoadCart();
    }

    public async void AddItem(int ProductId, int Quantity)
    {
        if (!UserSession.IsLoggedIn())
        {
            if (MessageText != null) MessageText.text = "Please log in first.";
            return;
        }

        string CartUrl = ServerRequest.SERVER_URL + "/addCartItem.php";

        Dictionary<string, string> CartData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "productId", ProductId.ToString() },
            { "quantity", Quantity.ToString() }
        };

        if (MessageText != null) MessageText.text = "Adding item...";

        string ResultText = await ServerRequest.SendPostRequest(CartUrl, CartData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        if (MessageText != null) MessageText.text = Result.message;
    }

    public async void LoadCart()
    {
        if (!UserSession.IsLoggedIn())
        {
            if (MessageText != null) MessageText.text = "Please log in first.";
            return;
        }

        string CartUrl = ServerRequest.SERVER_URL + "/getCart.php?userId=" + UserSession.UserId;
        if (MessageText != null) MessageText.text = "Loading cart...";

        string ResultText = await ServerRequest.SendGetRequest(CartUrl);
        CartResponse Result = JsonUtility.FromJson<CartResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            CartItems = Result.cartItems;
            if (SubtotalText != null)
                SubtotalText.text = "Subtotal: $" + Result.subtotal.ToString("0.00");

            ShowCart();
        }
        else
        {
            CartItems = new CartItemData[0];
            if (SubtotalText != null)
                SubtotalText.text = "Subtotal: $0.00";
            ShowCart();
        }
    }

    void ShowCart()
    {
        if (CartContainer == null) return;

        // Clear existing children
        foreach (Transform child in CartContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (CartItemData item in CartItems)
        {
            // Create a row container
            GameObject row = new GameObject("CartItem_" + item.cartItemId, typeof(RectTransform));
            row.transform.SetParent(CartContainer, false);
            
            RectTransform rt = row.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, 80);
            
            // Add a horizontal layout group for easy alignment
            var hlg = row.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = true;
            hlg.spacing = 15;
            hlg.padding = new RectOffset(10, 10, 5, 5);

            // Add background image (makes it readable and styled)
            var img = row.AddComponent<UnityEngine.UI.Image>();
            img.color = new Color(0.15f, 0.15f, 0.15f, 0.85f);

            // Product Thumbnail Image (far-left)
            GameObject thumbObj = new GameObject("Thumbnail", typeof(RectTransform));
            thumbObj.transform.SetParent(row.transform, false);
            
            var thumbImg = thumbObj.AddComponent<UnityEngine.UI.Image>();
            thumbImg.color = Color.clear; // Transparent until loaded
            
            var thumbLayout = thumbObj.AddComponent<UnityEngine.UI.LayoutElement>();
            thumbLayout.preferredWidth = 60;
            thumbLayout.preferredHeight = 60;

            if (!string.IsNullOrEmpty(item.imageUrl))
            {
                LoadCartImage(item.imageUrl, thumbImg);
            }

            // Item Name text
            GameObject nameObj = new GameObject("Name", typeof(RectTransform));
            nameObj.transform.SetParent(row.transform, false);
            var nameText = nameObj.AddComponent<TextMeshProUGUI>();
            nameText.text = item.itemName;
            nameText.fontSize = 18;
            nameText.alignment = TextAlignmentOptions.Left;
            
            // Allow name to stretch and take up remaining space
            var nameLayout = nameObj.AddComponent<UnityEngine.UI.LayoutElement>();
            nameLayout.flexibleWidth = 1f;

            // Price/Qty text
            GameObject qtyObj = new GameObject("Qty", typeof(RectTransform));
            qtyObj.transform.SetParent(row.transform, false);
            var qtyText = qtyObj.AddComponent<TextMeshProUGUI>();
            qtyText.text = $"${item.price:0.00} x {item.quantity}";
            qtyText.fontSize = 16;
            qtyText.alignment = TextAlignmentOptions.Center;
            
            var qtyLayout = qtyObj.AddComponent<UnityEngine.UI.LayoutElement>();
            qtyLayout.preferredWidth = 120;

            // Total text
            GameObject totalObj = new GameObject("Total", typeof(RectTransform));
            totalObj.transform.SetParent(row.transform, false);
            var totalText = totalObj.AddComponent<TextMeshProUGUI>();
            totalText.text = $"${item.lineTotal:0.00}";
            totalText.fontSize = 18;
            totalText.fontStyle = FontStyles.Bold;
            totalText.alignment = TextAlignmentOptions.Right;

            var totalLayout = totalObj.AddComponent<UnityEngine.UI.LayoutElement>();
            totalLayout.preferredWidth = 100;

            // Buttons Container
            GameObject btnGroup = new GameObject("Buttons", typeof(RectTransform));
            btnGroup.transform.SetParent(row.transform, false);
            var btnHlg = btnGroup.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
            btnHlg.spacing = 5;
            btnHlg.childControlWidth = true;
            btnHlg.childControlHeight = true;
            btnHlg.childForceExpandWidth = true;
            btnHlg.childForceExpandHeight = true;

            var btnGroupLayout = btnGroup.AddComponent<UnityEngine.UI.LayoutElement>();
            btnGroupLayout.preferredWidth = 90;

            // Decrease/Delete button (-)
            GameObject deleteObj = new GameObject("DeleteBtn", typeof(RectTransform));
            deleteObj.transform.SetParent(btnGroup.transform, false);
            var delImg = deleteObj.AddComponent<UnityEngine.UI.Image>();
            delImg.color = new Color(0.8f, 0.2f, 0.2f, 0.8f);
            var delBtn = deleteObj.AddComponent<UnityEngine.UI.Button>();
            
            var delLayout = deleteObj.AddComponent<UnityEngine.UI.LayoutElement>();
            delLayout.preferredWidth = 40;
            delLayout.preferredHeight = 40;
            
            GameObject delTextObj = new GameObject("Text", typeof(RectTransform));
            delTextObj.transform.SetParent(deleteObj.transform, false);
            
            RectTransform delTextRt = delTextObj.GetComponent<RectTransform>();
            delTextRt.anchorMin = Vector2.zero;
            delTextRt.anchorMax = Vector2.one;
            delTextRt.sizeDelta = Vector2.zero;

            var delTxt = delTextObj.AddComponent<TextMeshProUGUI>();
            delTxt.text = "-";
            delTxt.fontSize = 20;
            delTxt.alignment = TextAlignmentOptions.Center;
            delTxt.color = Color.white;
            delTxt.raycastTarget = false; // Prevent blocking clicks
            
            int cItemId = item.cartItemId;
            int currentQty = item.quantity;
            delBtn.onClick.AddListener(() => {
                if (currentQty > 1)
                    UpdateItem(cItemId, currentQty - 1);
                else
                    DeleteItem(cItemId);
            });

            // Increase button (+)
            GameObject addObj = new GameObject("AddBtn", typeof(RectTransform));
            addObj.transform.SetParent(btnGroup.transform, false);
            var addImg = addObj.AddComponent<UnityEngine.UI.Image>();
            addImg.color = new Color(0.2f, 0.8f, 0.2f, 0.8f);
            var addBtn = addObj.AddComponent<UnityEngine.UI.Button>();
            
            var addLayout = addObj.AddComponent<UnityEngine.UI.LayoutElement>();
            addLayout.preferredWidth = 40;
            addLayout.preferredHeight = 40;
            
            GameObject addTextObj = new GameObject("Text", typeof(RectTransform));
            addTextObj.transform.SetParent(addObj.transform, false);
            
            RectTransform addTextRt = addTextObj.GetComponent<RectTransform>();
            addTextRt.anchorMin = Vector2.zero;
            addTextRt.anchorMax = Vector2.one;
            addTextRt.sizeDelta = Vector2.zero;

            var addTxt = addTextObj.AddComponent<TextMeshProUGUI>();
            addTxt.text = "+";
            addTxt.fontSize = 20;
            addTxt.alignment = TextAlignmentOptions.Center;
            addTxt.color = Color.white;
            addTxt.raycastTarget = false; // Prevent blocking clicks
            
            addBtn.onClick.AddListener(() => {
                UpdateItem(cItemId, currentQty + 1);
            });
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

        if (MessageText != null) MessageText.text = "Updating quantity...";

        string ResultText = await ServerRequest.SendPostRequest(CartUrl, CartData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        if (MessageText != null) MessageText.text = Result.message;

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

        if (MessageText != null) MessageText.text = "Removing item...";

        string ResultText = await ServerRequest.SendPostRequest(CartUrl, CartData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
            LoadCart();
    }

    private async void LoadCartImage(string url, UnityEngine.UI.Image imageComponent)
    {
        Sprite sprite = await ServerRequest.GetSprite(url);
        if (sprite != null && imageComponent != null)
        {
            imageComponent.color = Color.white; // Restore opacity
            imageComponent.sprite = sprite;
        }
    }
}
