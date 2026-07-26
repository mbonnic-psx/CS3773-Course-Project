<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_POST["userId"] ?? 0);
$CartItemId = intval($_POST["cartItemId"] ?? 0);
$Quantity = intval($_POST["quantity"] ?? 0);

if ($Quantity <= 0) {
    echo json_encode(array("success" => false, "message" => "Quantity must be at least 1."));
    exit();
}

$UpdateQuery = $conn->prepare(
    "UPDATE cart_items c
     JOIN products p ON c.product_id = p.product_id
     SET c.quantity = ?
     WHERE c.cart_item_id = ? AND c.user_id = ? AND ? <= p.quantity_available"
);
$UpdateQuery->bind_param("iiii", $Quantity, $CartItemId, $UserId, $Quantity);
$UpdateQuery->execute();

if ($UpdateQuery->affected_rows > 0) {
    echo json_encode(array("success" => true, "message" => "Cart updated."));
} else {
    echo json_encode(array("success" => false, "message" => "Quantity exceeds availability or item was not found."));
}

$conn->close();
?>
