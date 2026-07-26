using System;
using UnityEngine;
using TMPro;

public class ProductManager : MonoBehaviour
{
    public TMP_InputField SearchInput;
    public TMP_Dropdown SortDropdown;
    public TMP_Text MessageText;

    public ProductData[] Products;

    public async void LoadProducts()
    {
        string SearchText = UnityEngine.Networking.UnityWebRequest.EscapeURL(SearchInput.text.Trim());
        string SortType = GetSortType();

        string ProductUrl = ServerRequest.SERVER_URL + "/getProducts.php?search=" +
                            SearchText + "&sort=" + SortType;

        string ResultText = await ServerRequest.SendGetRequest(ProductUrl);
        ProductResponse Result = JsonUtility.FromJson<ProductResponse>(ResultText);

        MessageText.text = Result.message;

        if (Result.success)
        {
            Products = Result.products;
            ShowProducts();
        }
    }

    string GetSortType()
    {
        if (SortDropdown.value == 1)
            return "priceLow";

        if (SortDropdown.value == 2)
            return "priceHigh";

        if (SortDropdown.value == 3)
            return "available";

        return "name";
    }

    void ShowProducts()
    {
        foreach (ProductData Product in Products)
        {
            Debug.Log(Product.itemName + " - $" + Product.price.ToString("0.00") +
                      " - Available: " + Product.quantityAvailable);
        }

        // Replace the Debug.Log code with your own product prefab creation code.
        // Each prefab can show Product.itemName, Product.price, Product.imageUrl,
        // Product.itemDescription and Product.quantityAvailable.
    }
}
