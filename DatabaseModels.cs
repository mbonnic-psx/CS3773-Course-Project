using System;

[Serializable]
public class BasicResponse
{
    public bool success;
    public string message;
}

[Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public int userId;
    public string email;
    public string username;
}

[Serializable]
public class ProductData
{
    public int productId;
    public string itemName;
    public string itemDescription;
    public float price;
    public int quantityAvailable;
    public string imageUrl;
}

[Serializable]
public class ProductResponse
{
    public bool success;
    public string message;
    public ProductData[] products;
}

[Serializable]
public class AddressData
{
    public int addressId;
    public string addressName;
    public string street;
    public string city;
    public string state;
    public string zipCode;
}

[Serializable]
public class AddressResponse
{
    public bool success;
    public string message;
    public AddressData[] addresses;
}

[Serializable]
public class CartItemData
{
    public int cartItemId;
    public int productId;
    public string itemName;
    public float price;
    public int quantity;
    public int quantityAvailable;
    public string imageUrl;
    public float lineTotal;
}

[Serializable]
public class CartResponse
{
    public bool success;
    public string message;
    public float subtotal;
    public CartItemData[] cartItems;
}

[Serializable]
public class DiscountResponse
{
    public bool success;
    public string message;
    public float discountPercent;
}

[Serializable]
public class OrderItemData
{
    public string itemName;
    public float itemPrice;
    public int quantity;
}

[Serializable]
public class OrderData
{
    public int orderId;
    public string deliveryType;
    public float subtotal;
    public float discountAmount;
    public float taxAmount;
    public float deliveryFee;
    public float totalAmount;
    public string orderDate;
    public OrderItemData[] orderItems;
}

[Serializable]
public class OrderHistoryResponse
{
    public bool success;
    public string message;
    public OrderData[] orders;
}

[Serializable]
public class PlaceOrderResponse
{
    public bool success;
    public string message;
    public int orderId;
    public float subtotal;
    public float discountAmount;
    public float taxAmount;
    public float deliveryFee;
    public float totalAmount;
}
