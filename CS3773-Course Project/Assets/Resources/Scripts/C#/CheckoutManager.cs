using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckoutManager : MonoBehaviour
{
    public UnityEngine.UI.InputField DiscountInput;
    public UnityEngine.UI.Dropdown DeliveryDropdown;
    public TMP_Text SubtotalText;
    public TMP_Text DiscountText;
    public TMP_Text TaxText;
    public TMP_Text DeliveryText;
    public TMP_Text TotalText;
    public TMP_Text MessageText;

    // Added new fields for features
    public TMP_Text SelectedAddressText;
    public Transform CheckoutItemsContainer;

    public float Subtotal;
    public float DiscountPercent;
    public int SelectedAddressId;

    private void Start()
    {
        // Setup layout container for checkout items if referenced
        if (CheckoutItemsContainer != null)
        {
            var vlg = CheckoutItemsContainer.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
            if (vlg == null) vlg = CheckoutItemsContainer.gameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.spacing = 6;
            vlg.padding = new RectOffset(5, 5, 5, 5);

            var csf = CheckoutItemsContainer.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            if (csf == null) csf = CheckoutItemsContainer.gameObject.AddComponent<UnityEngine.UI.ContentSizeFitter>();
            csf.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
        }

        if (DeliveryDropdown != null)
        {
            DeliveryDropdown.onValueChanged.AddListener((val) => CalculateSummary());
        }
        LoadCartAndCalculate();
    }

    public async void LoadCartAndCalculate()
    {
        if (!UserSession.IsLoggedIn())
        {
            if (MessageText != null) MessageText.text = "Please log in first.";
            return;
        }

        // Fetch addresses to select one
        LoadAddressesForCheckout();

        string CartUrl = ServerRequest.SERVER_URL + "/getCart.php?userId=" + UserSession.UserId;
        if (MessageText != null) MessageText.text = "Loading cart details...";

        string ResultText = await ServerRequest.SendGetRequest(CartUrl);
        CartResponse Result = JsonUtility.FromJson<CartResponse>(ResultText);

        if (Result.success)
        {
            SetSubtotal(Result.subtotal);
            ShowCheckoutItems(Result.cartItems);
        }
        else
        {
            SetSubtotal(0);
            if (MessageText != null) MessageText.text = "Cart is empty.";
            ShowCheckoutItems(new CartItemData[0]);
        }
    }

    public async void LoadAddressesForCheckout()
    {
        string AddressUrl = ServerRequest.SERVER_URL + "/getAddresses.php?userId=" + UserSession.UserId;
        string ResultText = await ServerRequest.SendGetRequest(AddressUrl);
        AddressResponse Result = JsonUtility.FromJson<AddressResponse>(ResultText);

        if (Result.success && Result.addresses != null && Result.addresses.Length > 0)
        {
            AddressData chosenAddress = null;

            // Check if there's a globally selected address from UserSession
            if (UserSession.SelectedAddressId > 0)
            {
                foreach (var addr in Result.addresses)
                {
                    if (addr.addressId == UserSession.SelectedAddressId)
                    {
                        chosenAddress = addr;
                        break;
                    }
                }
            }

            // Fallback to first address if no matching chosen address
            if (chosenAddress == null)
            {
                chosenAddress = Result.addresses[0];
            }

            SetAddress(chosenAddress.addressId);

            if (SelectedAddressText != null)
            {
                SelectedAddressText.text = $"<b>SHIPPING TO:</b>\n" +
                                            $"Name: {chosenAddress.addressName}\n" +
                                            $"{chosenAddress.street}\n" +
                                            $"{chosenAddress.city}, {chosenAddress.state} {chosenAddress.zipCode}";
            }
            else if (MessageText != null)
            {
                MessageText.text = "Shipping to: " + chosenAddress.street + ", " + chosenAddress.city;
            }
        }
        else
        {
            SetAddress(0);
            if (SelectedAddressText != null)
            {
                SelectedAddressText.text = "<b>SHIPPING TO:</b>\nNo shipping address selected.\nClick 'Add/Edit Address' at top right!";
            }
            else if (MessageText != null)
            {
                MessageText.text = "No shipping address found. Please add one first.";
            }
        }
    }

    private void ShowCheckoutItems(CartItemData[] items)
    {
        if (CheckoutItemsContainer == null) return;

        // Clear existing cloned children
        foreach (Transform child in CheckoutItemsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in items)
        {
            GameObject row = new GameObject("CheckoutItem_" + item.cartItemId, typeof(RectTransform));
            row.transform.SetParent(CheckoutItemsContainer, false);

            RectTransform rt = row.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, 50);

            var hlg = row.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = true;
            hlg.spacing = 10;
            hlg.padding = new RectOffset(5, 5, 2, 2);

            var img = row.AddComponent<UnityEngine.UI.Image>();
            img.color = new Color(0.15f, 0.18f, 0.28f, 0.85f); // Panel Navy matching theme

            // Image Thumbnail
            GameObject thumbObj = new GameObject("Thumbnail", typeof(RectTransform));
            thumbObj.transform.SetParent(row.transform, false);

            var thumbImg = thumbObj.AddComponent<UnityEngine.UI.Image>();
            thumbImg.color = Color.clear;

            var thumbLayout = thumbObj.AddComponent<UnityEngine.UI.LayoutElement>();
            thumbLayout.preferredWidth = 40;
            thumbLayout.preferredHeight = 40;

            if (!string.IsNullOrEmpty(item.imageUrl))
            {
                LoadCheckoutItemImage(item.imageUrl, thumbImg);
            }

            // Name
            GameObject nameObj = new GameObject("Name", typeof(RectTransform));
            nameObj.transform.SetParent(row.transform, false);
            var nameTxt = nameObj.AddComponent<TextMeshProUGUI>();
            nameTxt.text = item.itemName;
            nameTxt.fontSize = 14;
            nameTxt.alignment = TextAlignmentOptions.Left;
            nameTxt.color = Color.white;

            var nameLayout = nameObj.AddComponent<UnityEngine.UI.LayoutElement>();
            nameLayout.flexibleWidth = 1f;

            // Qty
            GameObject qtyObj = new GameObject("Qty", typeof(RectTransform));
            qtyObj.transform.SetParent(row.transform, false);
            var qtyTxt = qtyObj.AddComponent<TextMeshProUGUI>();
            qtyTxt.text = "x" + item.quantity;
            qtyTxt.fontSize = 13;
            qtyTxt.alignment = TextAlignmentOptions.Center;
            qtyTxt.color = new Color(0f, 0.706f, 0.847f, 1f); // Electric Blue

            var qtyLayout = qtyObj.AddComponent<UnityEngine.UI.LayoutElement>();
            qtyLayout.preferredWidth = 40;

            // Total
            GameObject totalObj = new GameObject("Total", typeof(RectTransform));
            totalObj.transform.SetParent(row.transform, false);
            var totalTxt = totalObj.AddComponent<TextMeshProUGUI>();
            totalTxt.text = $"${item.lineTotal:0.00}";
            totalTxt.fontSize = 14;
            totalTxt.fontStyle = FontStyles.Bold;
            totalTxt.alignment = TextAlignmentOptions.Right;
            totalTxt.color = Color.white;

            var totalLayout = totalObj.AddComponent<UnityEngine.UI.LayoutElement>();
            totalLayout.preferredWidth = 70;
        }
    }

    private async void LoadCheckoutItemImage(string url, UnityEngine.UI.Image imageComponent)
    {
        Sprite sprite = await ServerRequest.GetSprite(url);
        if (sprite != null && imageComponent != null)
        {
            imageComponent.color = Color.white;
            imageComponent.sprite = sprite;
        }
    }

    public void SetSubtotal(float NewSubtotal)
    {
        Subtotal = NewSubtotal;
        CalculateSummary();
    }

    public void SetAddress(int AddressId)
    {
        SelectedAddressId = AddressId;
        UserSession.SelectedAddressId = AddressId;
    }

    public async void ApplyDiscount()
    {
        if (DiscountInput == null) return;

        string code = DiscountInput.text.Trim().ToUpper();
        if (string.IsNullOrEmpty(code))
        {
            DiscountPercent = 0;
            CalculateSummary();
            return;
        }

        string DiscountCode = UnityEngine.Networking.UnityWebRequest.EscapeURL(code);

        string DiscountUrl = ServerRequest.SERVER_URL + "/getDiscount.php?code=" + DiscountCode;
        string ResultText = await ServerRequest.SendGetRequest(DiscountUrl);
        DiscountResponse Result = JsonUtility.FromJson<DiscountResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
            DiscountPercent = Result.discountPercent;
        else
            DiscountPercent = 0;

        CalculateSummary();
    }

    public void CalculateSummary()
    {
        float DiscountAmount = Subtotal * (DiscountPercent / 100f);
        float TaxableAmount = Subtotal - DiscountAmount;
        float TaxAmount = TaxableAmount * 0.0825f;
        float DeliveryFee = GetDeliveryFee();
        float TotalAmount = TaxableAmount + TaxAmount + DeliveryFee;

        if (SubtotalText != null) SubtotalText.text = "Subtotal: $" + Subtotal.ToString("0.00");
        if (DiscountText != null) DiscountText.text = "Discount: -$" + DiscountAmount.ToString("0.00");
        if (TaxText != null) TaxText.text = "Tax: $" + TaxAmount.ToString("0.00");
        if (DeliveryText != null) DeliveryText.text = "Delivery: $" + DeliveryFee.ToString("0.00");
        if (TotalText != null) TotalText.text = "Total: $" + TotalAmount.ToString("0.00");
    }

    float GetDeliveryFee()
    {
        if (DeliveryDropdown == null) return 4.99f;

        if (DeliveryDropdown.value == 0)
            return 4.99f;

        if (DeliveryDropdown.value == 1)
            return 9.99f;

        return 0f;
    }

    string GetDeliveryType()
    {
        if (DeliveryDropdown == null) return "Standard";

        if (DeliveryDropdown.value == 0)
            return "Standard";

        if (DeliveryDropdown.value == 1)
            return "Express";

        return "Pickup";
    }

    public async void PlaceOrder()
    {
        if (SelectedAddressId <= 0)
        {
            if (MessageText != null) MessageText.text = "Please select or add a shipping address.";
            return;
        }

        string OrderUrl = ServerRequest.SERVER_URL + "/placeOrder.php";

        string discountCode = "";
        if (DiscountInput != null)
        {
            discountCode = DiscountInput.text.Trim().ToUpper();
        }

        Dictionary<string, string> OrderData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "addressId", SelectedAddressId.ToString() },
            { "deliveryType", GetDeliveryType() },
            { "discountCode", discountCode }
        };

        if (MessageText != null) MessageText.text = "Placing order...";

        string ResultText = await ServerRequest.SendPostRequest(OrderUrl, OrderData);
        PlaceOrderResponse Result = JsonUtility.FromJson<PlaceOrderResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            if (TotalText != null)
            {
                TotalText.text = "Order #" + Result.orderId +
                                 " Total: $" + Result.totalAmount.ToString("0.00");
            }
            Invoke("GoToOrderHistory", 2.0f);
        }
    }

    void GoToOrderHistory()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OrderHistoryScene");
    }
}
