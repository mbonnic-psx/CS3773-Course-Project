<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_GET["userId"] ?? 0);

$CartQuery = $conn->prepare(
    "SELECT c.cart_item_id, c.product_id, c.quantity,
            p.item_name, p.price, p.image_url, p.quantity_available
     FROM cart_items c
     JOIN products p ON c.product_id = p.product_id
     WHERE c.user_id = ?
     ORDER BY c.cart_item_id DESC"
);
$CartQuery->bind_param("i", $UserId);
$CartQuery->execute();
$CartResult = $CartQuery->get_result();

$CartItems = array();
$Subtotal = 0;

while ($CartRow = $CartResult->fetch_assoc()) {
    $LineTotal = floatval($CartRow["price"]) * intval($CartRow["quantity"]);
    $Subtotal += $LineTotal;

    $CartItems[] = array(
        "cartItemId" => intval($CartRow["cart_item_id"]),
        "productId" => intval($CartRow["product_id"]),
        "itemName" => $CartRow["item_name"],
        "price" => floatval($CartRow["price"]),
        "quantity" => intval($CartRow["quantity"]),
        "quantityAvailable" => intval($CartRow["quantity_available"]),
        "imageUrl" => $CartRow["image_url"],
        "lineTotal" => $LineTotal
    );
}

echo json_encode(array(
    "success" => true,
    "message" => "Cart loaded.",
    "subtotal" => $Subtotal,
    "cartItems" => $CartItems
));

$conn->close();
?>
