using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckoutManager : MonoBehaviour
{
    public TMP_InputField DiscountInput;
    public TMP_Dropdown DeliveryDropdown;
    public TMP_Text SubtotalText;
    public TMP_Text DiscountText;
    public TMP_Text TaxText;
    public TMP_Text DeliveryText;
    public TMP_Text TotalText;
    public TMP_Text MessageText;

    public float Subtotal;
    public float DiscountPercent;
    public int SelectedAddressId;

    public void SetSubtotal(float NewSubtotal)
    {
        Subtotal = NewSubtotal;
        CalculateSummary();
    }

    public void SetAddress(int AddressId)
    {
        SelectedAddressId = AddressId;
    }

    public async void ApplyDiscount()
    {
        string DiscountCode = UnityEngine.Networking.UnityWebRequest.EscapeURL(
            DiscountInput.text.Trim().ToUpper()
        );

        string DiscountUrl = ServerRequest.SERVER_URL + "/getDiscount.php?code=" + DiscountCode;
        string ResultText = await ServerRequest.SendGetRequest(DiscountUrl);
        DiscountResponse Result = JsonUtility.FromJson<DiscountResponse>(ResultText);

        MessageText.text = Result.message;

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

        SubtotalText.text = "Subtotal: $" + Subtotal.ToString("0.00");
        DiscountText.text = "Discount: -$" + DiscountAmount.ToString("0.00");
        TaxText.text = "Tax: $" + TaxAmount.ToString("0.00");
        DeliveryText.text = "Delivery: $" + DeliveryFee.ToString("0.00");
        TotalText.text = "Total: $" + TotalAmount.ToString("0.00");
    }

    float GetDeliveryFee()
    {
        if (DeliveryDropdown.value == 0)
            return 4.99f;

        if (DeliveryDropdown.value == 1)
            return 9.99f;

        return 0f;
    }

    string GetDeliveryType()
    {
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
            MessageText.text = "Please select an address.";
            return;
        }

        string OrderUrl = ServerRequest.SERVER_URL + "/placeOrder.php";

        Dictionary<string, string> OrderData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "addressId", SelectedAddressId.ToString() },
            { "deliveryType", GetDeliveryType() },
            { "discountCode", DiscountInput.text.Trim().ToUpper() }
        };

        string ResultText = await ServerRequest.SendPostRequest(OrderUrl, OrderData);
        PlaceOrderResponse Result = JsonUtility.FromJson<PlaceOrderResponse>(ResultText);

        MessageText.text = Result.message;

        if (Result.success)
        {
            TotalText.text = "Order #" + Result.orderId +
                             " Total: $" + Result.totalAmount.ToString("0.00");
        }
    }
}
