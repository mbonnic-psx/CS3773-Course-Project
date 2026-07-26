using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddressManager : MonoBehaviour
{
    public TMP_InputField AddressNameInput;
    public TMP_InputField StreetInput;
    public TMP_InputField CityInput;
    public TMP_InputField StateInput;
    public TMP_InputField ZipInput;
    public TMP_Text MessageText;

    public AddressData[] Addresses;

    public async void AddAddress()
    {
        string AddressUrl = ServerRequest.SERVER_URL + "/addAddress.php";

        Dictionary<string, string> AddressData = new Dictionary<string, string>()
        {
            { "userId", UserSession.UserId.ToString() },
            { "addressName", AddressNameInput.text.Trim() },
            { "street", StreetInput.text.Trim() },
            { "city", CityInput.text.Trim() },
            { "state", StateInput.text.Trim() },
            { "zipCode", ZipInput.text.Trim() }
        };

        string ResultText = await ServerRequest.SendPostRequest(AddressUrl, AddressData);
        BasicResponse Result = JsonUtility.FromJson<BasicResponse>(ResultText);
        MessageText.text = Result.message;

        if (Result.success)
            LoadAddresses();
    }

    public async void LoadAddresses()
    {
        string AddressUrl = ServerRequest.SERVER_URL + "/getAddresses.php?userId=" + UserSession.UserId;
        string ResultText = await ServerRequest.SendGetRequest(AddressUrl);
        AddressResponse Result = JsonUtility.FromJson<AddressResponse>(ResultText);

        MessageText.text = Result.message;

        if (Result.success)
        {
            Addresses = Result.addresses;

            foreach (AddressData Address in Addresses)
            {
                Debug.Log(Address.addressName + ": " + Address.street + ", " +
                          Address.city + ", " + Address.state + " " + Address.zipCode);
            }
        }
    }
}
