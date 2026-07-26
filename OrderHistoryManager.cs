using UnityEngine;
using TMPro;

public class OrderHistoryManager : MonoBehaviour
{
    public TMP_Dropdown SortDropdown;
    public TMP_Text MessageText;

    public OrderData[] Orders;

    public async void LoadOrderHistory()
    {
        string SortType = GetSortType();

        string OrderUrl = ServerRequest.SERVER_URL + "/getOrderHistory.php?userId=" +
                          UserSession.UserId + "&sort=" + SortType;

        string ResultText = await ServerRequest.SendGetRequest(OrderUrl);
        OrderHistoryResponse Result = JsonUtility.FromJson<OrderHistoryResponse>(ResultText);

        MessageText.text = Result.message;

        if (Result.success)
        {
            Orders = Result.orders;
            ShowOrders();
        }
    }

    string GetSortType()
    {
        if (SortDropdown.value == 1)
            return "dateOld";

        if (SortDropdown.value == 2)
            return "totalLow";

        if (SortDropdown.value == 3)
            return "totalHigh";

        return "dateNew";
    }

    void ShowOrders()
    {
        foreach (OrderData Order in Orders)
        {
            Debug.Log("Order #" + Order.orderId + " - " + Order.orderDate +
                      " - $" + Order.totalAmount.ToString("0.00"));

            foreach (OrderItemData Item in Order.orderItems)
            {
                Debug.Log(Item.itemName + " x" + Item.quantity);
            }
        }

        // Replace Debug.Log with your own order history prefab code.
    }
}
