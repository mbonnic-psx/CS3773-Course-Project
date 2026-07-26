using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddressManager : MonoBehaviour
{
    public UnityEngine.UI.InputField AddressNameInput;
    public UnityEngine.UI.InputField StreetInput;
    public UnityEngine.UI.InputField CityInput;
    public UnityEngine.UI.InputField StateInput;
    public UnityEngine.UI.InputField ZipInput;
    public TMP_Text MessageText;
    public Transform AddressContainer;

    public AddressData[] Addresses;
    public int SelectedAddressId = 0;

    private List<GameObject> addressCards = new List<GameObject>();

    private void Start()
    {
        SelectedAddressId = UserSession.SelectedAddressId;

        // Set up layout on AddressContainer dynamically if needed
        if (AddressContainer != null)
        {
            var vlg = AddressContainer.GetComponent<UnityEngine.UI.VerticalLayoutGroup>();
            if (vlg == null) vlg = AddressContainer.gameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();
            vlg.childControlWidth = true;
            vlg.childControlHeight = false;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.spacing = 5;
            vlg.padding = new RectOffset(5, 5, 5, 5);

            var csf = AddressContainer.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            if (csf == null) csf = AddressContainer.gameObject.AddComponent<UnityEngine.UI.ContentSizeFitter>();
            csf.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
        }

        LoadAddresses();
    }

    public async void AddAddress()
    {
        if (StreetInput == null || CityInput == null || StateInput == null || ZipInput == null)
        {
            Debug.LogError("Required address fields are not referenced!");
            return;
        }

        string streetText = StreetInput.text.Trim();
        string cityText = CityInput.text.Trim();
        string stateText = StateInput.text.Trim();
        string zipText = ZipInput.text.Trim();

        if (streetText == "" || cityText == "" || stateText == "" || zipText == "")
        {
            if (MessageText != null) MessageText.text = "Please fill in all address fields.";
            return;
        }

        string addressName = "Home";
        if (AddressNameInput != null && !string.IsNullOrEmpty(AddressNameInput.text))
        {
            addressName = AddressNameInput.text.Trim();
        }
        else
        {
            addressName = streetText; // Fallback to street name if no name input is in the scene
        }

        string AddressUrl = ServerRequest.SERVER_URL + "/addAddress.php";

        Dictionary<string, string> AddressData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "addressName", addressName },
            { "street", streetText },
            { "city", cityText },
            { "state", stateText },
            { "zipCode", zipText }
        };

        if (MessageText != null) MessageText.text = "Adding address...";

        string ResultText = await ServerRequest.SendPostRequest(AddressUrl, AddressData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            // Clear inputs
            if (AddressNameInput != null) AddressNameInput.text = "";
            StreetInput.text = "";
            CityInput.text = "";
            StateInput.text = "";
            ZipInput.text = "";

            LoadAddresses();
        }
    }

    public async void LoadAddresses()
    {
        if (!UserSession.IsLoggedIn())
        {
            if (MessageText != null) MessageText.text = "Please log in first.";
            return;
        }

        string AddressUrl = ServerRequest.SERVER_URL + "/getAddresses.php?userId=" + UserSession.UserId;
        if (MessageText != null) MessageText.text = "Loading addresses...";

        string ResultText = await ServerRequest.SendGetRequest(AddressUrl);
        AddressResponse Result = JsonUtility.FromJson<AddressResponse>(ResultText);

        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            Addresses = Result.addresses;
            ShowAddresses();
        }
        else
        {
            Addresses = new AddressData[0];
            ShowAddresses();
        }
    }

    void ShowAddresses()
    {
        if (AddressContainer == null) return;

        addressCards.Clear();
        foreach (Transform child in AddressContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (AddressData address in Addresses)
        {
            GameObject card = new GameObject("Address_" + address.addressId, typeof(RectTransform));
            card.transform.SetParent(AddressContainer, false);
            
            RectTransform rt = card.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, 60);

            var img = card.AddComponent<UnityEngine.UI.Image>();
            img.color = new Color(0.15f, 0.15f, 0.15f, 0.85f);

            var btn = card.AddComponent<UnityEngine.UI.Button>();

            // Add text for Address Details
            GameObject textObj = new GameObject("Details", typeof(RectTransform));
            textObj.transform.SetParent(card.transform, false);
            
            RectTransform textRt = textObj.GetComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.sizeDelta = Vector2.zero;
            textRt.offsetMin = new Vector2(10, 5);
            textRt.offsetMax = new Vector2(-10, -5);

            var textComp = textObj.AddComponent<TextMeshProUGUI>();
            textComp.text = $"<b>{address.addressName}</b>\n{address.street}, {address.city}, {address.state} {address.zipCode}";
            textComp.fontSize = 14;
            textComp.color = Color.white;
            textComp.alignment = TextAlignmentOptions.MidlineLeft;

            int addrId = address.addressId;
            btn.onClick.AddListener(() => SelectAddress(addrId, card));

            addressCards.Add(card);

            // Auto-select first address if none selected
            if (SelectedAddressId == 0)
            {
                SelectAddress(addrId, card);
            }
            else if (SelectedAddressId == addrId)
            {
                SelectAddress(addrId, card);
            }
        }
    }

    void SelectAddress(int addressId, GameObject selectedCard)
    {
        SelectedAddressId = addressId;
        UserSession.SelectedAddressId = addressId;

        // Visual feedback: highlight selected, dim others
        foreach (var card in addressCards)
        {
            var img = card.GetComponent<UnityEngine.UI.Image>();
            if (card == selectedCard)
            {
                img.color = new Color(0.2f, 0.5f, 0.2f, 0.9f); // Green-ish highlight
            }
            else
            {
                img.color = new Color(0.15f, 0.15f, 0.15f, 0.85f);
            }
        }
    }

    public async void DeleteAddress()
    {
        if (SelectedAddressId <= 0)
        {
            if (MessageText != null) MessageText.text = "Please select an address to delete.";
            return;
        }

        string AddressUrl = ServerRequest.SERVER_URL + "/deleteAddress.php";

        Dictionary<string, string> AddressData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "addressId", SelectedAddressId.ToString() }
        };

        if (MessageText != null) MessageText.text = "Deleting address...";

        string ResultText = await ServerRequest.SendPostRequest(AddressUrl, AddressData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        if (MessageText != null) MessageText.text = Result.message;

        if (Result.success)
        {
            SelectedAddressId = 0;
            UserSession.SelectedAddressId = 0;
            LoadAddresses();
        }
    }
}
