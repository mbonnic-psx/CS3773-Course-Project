using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProductManager : MonoBehaviour
{
    public UnityEngine.UI.InputField SearchInput;
    public TMP_Dropdown SortDropdown;
    public TMP_Text MessageText;

    public GameObject ProductCardTemplate;
    public Transform ProductContainer;

    public ProductData[] Products;

    private void Start()
    {
        // Set up grid layout on ProductContainer dynamically to display cards in a neat grid wrap
        if (ProductContainer != null)
        {
            var glg = ProductContainer.GetComponent<UnityEngine.UI.GridLayoutGroup>();
            if (glg == null) glg = ProductContainer.gameObject.AddComponent<UnityEngine.UI.GridLayoutGroup>();
            
            glg.cellSize = new Vector2(183f, 150f);
            glg.spacing = new Vector2(15f, 15f);
            glg.startCorner = UnityEngine.UI.GridLayoutGroup.Corner.UpperLeft;
            glg.startAxis = UnityEngine.UI.GridLayoutGroup.Axis.Horizontal;
            glg.childAlignment = TextAnchor.UpperLeft;

            var csf = ProductContainer.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            if (csf == null) csf = ProductContainer.gameObject.AddComponent<UnityEngine.UI.ContentSizeFitter>();
            csf.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            csf.horizontalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained;
        }

        LoadProducts();
        
        // Listen to search changes and dropdown changes to reload
        if (SearchInput != null) SearchInput.onEndEdit.AddListener((val) => LoadProducts());
        if (SortDropdown != null) SortDropdown.onValueChanged.AddListener((val) => LoadProducts());
    }

    public async void LoadProducts()
    {
        string SearchText = "";
        if (SearchInput != null)
        {
            SearchText = UnityEngine.Networking.UnityWebRequest.EscapeURL(SearchInput.text.Trim());
        }

        string SortType = GetSortType();

        string ProductUrl = ServerRequest.SERVER_URL + "/getProducts.php?search=" +
                            SearchText + "&sort=" + SortType;

        if (MessageText != null) MessageText.text = "Loading products...";

        string ResultText = await ServerRequest.SendGetRequest(ProductUrl);
        ProductResponse Result = JsonUtility.FromJson<ProductResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            Products = Result.products;
            ShowProducts();
        }
    }

    string GetSortType()
    {
        if (SortDropdown == null) return "name";

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
        if (ProductCardTemplate == null || ProductContainer == null)
        {
            Debug.LogWarning("ProductCardTemplate or ProductContainer is not referenced.");
            return;
        }

        // Hide template itself
        ProductCardTemplate.SetActive(false);

        // Clear existing instantiated cards (except the template)
        foreach (Transform child in ProductContainer)
        {
            if (child.gameObject != ProductCardTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (ProductData Product in Products)
        {
            GameObject card = Instantiate(ProductCardTemplate, ProductContainer);
            card.SetActive(true);

            // Find text components
            TMP_Text[] textComponents = card.GetComponentsInChildren<TMP_Text>();
            foreach (var textComp in textComponents)
            {
                if (textComp.gameObject.name == "ProductName")
                    textComp.text = Product.itemName;
                else if (textComp.gameObject.name == "ProductDescription")
                    textComp.text = Product.itemDescription;
                else if (textComp.gameObject.name == "PriceText")
                    textComp.text = "$" + Product.price.ToString("0.00");
            }

            // Find and set ProductImage
            UnityEngine.UI.Image imgComp = null;
            var images = card.GetComponentsInChildren<UnityEngine.UI.Image>();
            foreach (var img in images)
            {
                if (img.gameObject.name == "ProductImage")
                {
                    imgComp = img;
                    break;
                }
            }

            if (imgComp != null && !string.IsNullOrEmpty(Product.imageUrl))
            {
                LoadProductImage(Product.imageUrl, imgComp);
            }

            // Setup AddToCartButton
            UnityEngine.UI.Button btn = card.GetComponentInChildren<UnityEngine.UI.Button>();
            if (btn != null)
            {
                int prodId = Product.productId;
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => AddProductToCart(prodId));
            }
        }
    }

    private async void LoadProductImage(string url, UnityEngine.UI.Image imageComponent)
    {
        Sprite sprite = await ServerRequest.GetSprite(url);
        if (sprite != null && imageComponent != null)
        {
            imageComponent.sprite = sprite;
        }
    }

    async void AddProductToCart(int productId)
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
            { "productId", productId.ToString() },
            { "quantity", "1" }
        };

        if (MessageText != null) MessageText.text = "Adding to cart...";

        string ResultText = await ServerRequest.SendPostRequest(CartUrl, CartData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        
        if (MessageText != null) MessageText.text = Result.message;
    }
}
