<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_POST["userId"] ?? 0);
$ProductId = intval($_POST["productId"] ?? 0);
$Quantity = intval($_POST["quantity"] ?? 1);

if ($UserId <= 0 || $ProductId <= 0 || $Quantity <= 0) {
    echo json_encode(array("success" => false, "message" => "Cart data is not valid."));
    exit();
}

$StockQuery = $conn->prepare("SELECT quantity_available FROM products WHERE product_id = ?");
$StockQuery->bind_param("i", $ProductId);
$StockQuery->execute();
$StockResult = $StockQuery->get_result();

if ($StockResult->num_rows == 0) {
    echo json_encode(array("success" => false, "message" => "Product was not found."));
    exit();
}

$StockRow = $StockResult->fetch_assoc();

if ($Quantity > intval($StockRow["quantity_available"])) {
    echo json_encode(array("success" => false, "message" => "Not enough items are available."));
    exit();
}

$CartQuery = $conn->prepare(
    "INSERT INTO cart_items (user_id, product_id, quantity)
     VALUES (?, ?, ?)
     ON DUPLICATE KEY UPDATE quantity = quantity + VALUES(quantity)"
);
$CartQuery->bind_param("iii", $UserId, $ProductId, $Quantity);

if ($CartQuery->execute()) {
    echo json_encode(array("success" => true, "message" => "Item added to cart."));
} else {
    echo json_encode(array("success" => false, "message" => "Item could not be added."));
}

$conn->close();
?>
