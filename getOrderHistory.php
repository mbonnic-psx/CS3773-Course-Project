<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_GET["userId"] ?? 0);
$SortType = $_GET["sort"] ?? "dateNew";

$SortSql = "order_date DESC";

if ($SortType == "dateOld") {
    $SortSql = "order_date ASC";
} else if ($SortType == "totalLow") {
    $SortSql = "total_amount ASC";
} else if ($SortType == "totalHigh") {
    $SortSql = "total_amount DESC";
}

$OrderQuery = $conn->prepare(
    "SELECT order_id, delivery_type, subtotal, discount_amount, tax_amount,
            delivery_fee, total_amount, order_date
     FROM orders WHERE user_id = ?
     ORDER BY " . $SortSql
);
$OrderQuery->bind_param("i", $UserId);
$OrderQuery->execute();
$OrderResult = $OrderQuery->get_result();

$Orders = array();

while ($OrderRow = $OrderResult->fetch_assoc()) {
    $OrderId = intval($OrderRow["order_id"]);

    $ItemQuery = $conn->prepare(
        "SELECT item_name, item_price, quantity FROM order_items WHERE order_id = ?"
    );
    $ItemQuery->bind_param("i", $OrderId);
    $ItemQuery->execute();
    $ItemResult = $ItemQuery->get_result();

    $OrderItems = array();

    while ($ItemRow = $ItemResult->fetch_assoc()) {
        $OrderItems[] = array(
            "itemName" => $ItemRow["item_name"],
            "itemPrice" => floatval($ItemRow["item_price"]),
            "quantity" => intval($ItemRow["quantity"])
        );
    }

    $Orders[] = array(
        "orderId" => $OrderId,
        "deliveryType" => $OrderRow["delivery_type"],
        "subtotal" => floatval($OrderRow["subtotal"]),
        "discountAmount" => floatval($OrderRow["discount_amount"]),
        "taxAmount" => floatval($OrderRow["tax_amount"]),
        "deliveryFee" => floatval($OrderRow["delivery_fee"]),
        "totalAmount" => floatval($OrderRow["total_amount"]),
        "orderDate" => $OrderRow["order_date"],
        "orderItems" => $OrderItems
    );
}

echo json_encode(array("success" => true, "message" => "Order history loaded.", "orders" => $Orders));
$conn->close();
?>
