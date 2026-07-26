using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderHistoryManager : MonoBehaviour
{
    public UnityEngine.UI.Dropdown SortDropdown;
    public TMP_Text MessageText;

    public GameObject OrderCardTemplate;
    public Transform OrderContainer;

    public OrderData[] Orders;

    private void Start()
    {
        // Set up layout on OrderContainer dynamically to prevent overlaps
        if (OrderContainer != null)
        {
            var vlg = OrderContainer.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
            if (vlg == null) vlg = OrderContainer.gameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
            
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.spacing = 20f;
            vlg.padding = new RectOffset(10, 10, 10, 10);

            var csf = OrderContainer.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            if (csf == null) csf = OrderContainer.gameObject.AddComponent<UnityEngine.UI.ContentSizeFitter>();
            csf.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
        }

        if (SortDropdown != null)
        {
            SortDropdown.onValueChanged.AddListener((val) => LoadOrderHistory());
        }
        LoadOrderHistory();
    }

    public async void LoadOrderHistory()
    {
        if (!UserSession.IsLoggedIn())
        {
            if (MessageText != null) MessageText.text = "Please log in first.";
            return;
        }

        string SortType = GetSortType();

        string OrderUrl = ServerRequest.SERVER_URL + "/getOrderHistory.php?userId=" +
                          UserSession.UserId + "&sort=" + SortType;

        if (MessageText != null) MessageText.text = "Loading order history...";

        string ResultText = await ServerRequest.SendGetRequest(OrderUrl);
        OrderHistoryResponse Result = JsonUtility.FromJson<OrderHistoryResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            Orders = Result.orders;
            ShowOrders();
        }
        else
        {
            Orders = new OrderData[0];
            ShowOrders();
        }
    }

    string GetSortType()
    {
        if (SortDropdown == null) return "dateNew";

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
        if (OrderCardTemplate == null || OrderContainer == null)
        {
            Debug.LogWarning("OrderCardTemplate or OrderContainer is not referenced.");
            return;
        }

        OrderCardTemplate.SetActive(false);

        // Clear existing cloned children
        foreach (Transform child in OrderContainer)
        {
            if (child.gameObject != OrderCardTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (OrderData Order in Orders)
        {
            GameObject card = Instantiate(OrderCardTemplate, OrderContainer);
            card.SetActive(true);

            TMP_Text[] textComponents = card.GetComponentsInChildren<TMP_Text>();
            foreach (var textComp in textComponents)
            {
                if (textComp.gameObject.name == "OrderIDText")
                    textComp.text = "Order #" + Order.orderId;
                else if (textComp.gameObject.name == "DateText")
                    textComp.text = Order.orderDate;
                else if (textComp.gameObject.name == "TotalText")
                    textComp.text = "Total: $" + Order.totalAmount.ToString("0.00");
                else if (textComp.gameObject.name == "StatusText")
                {
                    // Build a summary of items
                    string itemsSummary = "";
                    if (Order.orderItems != null)
                    {
                        List<string> items = new List<string>();
                        foreach (var item in Order.orderItems)
                        {
                            items.Add(item.itemName + " (x" + item.quantity + ")");
                        }
                        itemsSummary = string.Join(", ", items);
                    }
                    textComp.text = "Items: " + itemsSummary;
                }
            }
        }
    }
}
