<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_POST["userId"] ?? 0);
$AddressId = intval($_POST["addressId"] ?? 0);
$DeliveryType = $_POST["deliveryType"] ?? "Standard";
$DiscountCode = strtoupper(trim($_POST["discountCode"] ?? ""));

if ($UserId <= 0 || $AddressId <= 0) {
    echo json_encode(array("success" => false, "message" => "User and address are required."));
    exit();
}

$DeliveryFee = 4.99;

if ($DeliveryType == "Pickup") {
    $DeliveryFee = 0.00;
} else if ($DeliveryType == "Express") {
    $DeliveryFee = 9.99;
}

$conn->begin_transaction();

try {
    $CartQuery = $conn->prepare(
        "SELECT c.product_id, c.quantity, p.item_name, p.price, p.quantity_available
         FROM cart_items c
         JOIN products p ON c.product_id = p.product_id
         WHERE c.user_id = ? FOR UPDATE"
    );
    $CartQuery->bind_param("i", $UserId);
    $CartQuery->execute();
    $CartResult = $CartQuery->get_result();

    if ($CartResult->num_rows == 0) {
        throw new Exception("Your cart is empty.");
    }

    $CartRows = array();
    $Subtotal = 0;

    while ($CartRow = $CartResult->fetch_assoc()) {
        if (intval($CartRow["quantity"]) > intval($CartRow["quantity_available"])) {
            throw new Exception($CartRow["item_name"] . " does not have enough stock.");
        }

        $Subtotal += floatval($CartRow["price"]) * intval($CartRow["quantity"]);
        $CartRows[] = $CartRow;
    }

    $DiscountPercent = 0;

    if ($DiscountCode != "") {
        $DiscountQuery = $conn->prepare(
            "SELECT discount_percent FROM discount_codes WHERE code = ? AND active = 1"
        );
        $DiscountQuery->bind_param("s", $DiscountCode);
        $DiscountQuery->execute();
        $DiscountResult = $DiscountQuery->get_result();

        if ($DiscountResult->num_rows > 0) {
            $DiscountRow = $DiscountResult->fetch_assoc();
            $DiscountPercent = floatval($DiscountRow["discount_percent"]);
        } else {
            throw new Exception("Discount code is not valid.");
        }
    }

    $DiscountAmount = round($Subtotal * ($DiscountPercent / 100), 2);
    $TaxableAmount = $Subtotal - $DiscountAmount;
    $TaxAmount = round($TaxableAmount * 0.0825, 2);
    $TotalAmount = round($TaxableAmount + $TaxAmount + $DeliveryFee, 2);

    $OrderQuery = $conn->prepare(
        "INSERT INTO orders
         (user_id, address_id, delivery_type, discount_code, subtotal,
          discount_amount, tax_amount, delivery_fee, total_amount)
         VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)"
    );
    $OrderQuery->bind_param(
        "iissddddd",
        $UserId,
        $AddressId,
        $DeliveryType,
        $DiscountCode,
        $Subtotal,
        $DiscountAmount,
        $TaxAmount,
        $DeliveryFee,
        $TotalAmount
    );
    $OrderQuery->execute();

    $OrderId = $conn->insert_id;

    foreach ($CartRows as $CartRow) {
        $ProductId = intval($CartRow["product_id"]);
        $ItemName = $CartRow["item_name"];
        $ItemPrice = floatval($CartRow["price"]);
        $Quantity = intval($CartRow["quantity"]);

        $OrderItemQuery = $conn->prepare(
            "INSERT INTO order_items (order_id, product_id, item_name, item_price, quantity)
             VALUES (?, ?, ?, ?, ?)"
        );
        $OrderItemQuery->bind_param("iisdi", $OrderId, $ProductId, $ItemName, $ItemPrice, $Quantity);
        $OrderItemQuery->execute();

        $StockQuery = $conn->prepare(
            "UPDATE products SET quantity_available = quantity_available - ? WHERE product_id = ?"
        );
        $StockQuery->bind_param("ii", $Quantity, $ProductId);
        $StockQuery->execute();
    }

    $ClearCartQuery = $conn->prepare("DELETE FROM cart_items WHERE user_id = ?");
    $ClearCartQuery->bind_param("i", $UserId);
    $ClearCartQuery->execute();

    $conn->commit();

    echo json_encode(array(
        "success" => true,
        "message" => "Order placed.",
        "orderId" => $OrderId,
        "subtotal" => $Subtotal,
        "discountAmount" => $DiscountAmount,
        "taxAmount" => $TaxAmount,
        "deliveryFee" => $DeliveryFee,
        "totalAmount" => $TotalAmount
    ));
} catch (Exception $e) {
    $conn->rollback();
    echo json_encode(array("success" => false, "message" => $e->getMessage()));
}

$conn->close();
?>
