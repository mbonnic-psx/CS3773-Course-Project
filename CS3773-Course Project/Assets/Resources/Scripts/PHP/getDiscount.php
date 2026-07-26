<?php
header("Content-Type: application/json");
require "db.php";

$Code = strtoupper(trim($_GET["code"] ?? ""));

$DiscountQuery = $conn->prepare(
    "SELECT discount_percent FROM discount_codes WHERE code = ? AND active = 1"
);
$DiscountQuery->bind_param("s", $Code);
$DiscountQuery->execute();
$DiscountResult = $DiscountQuery->get_result();

if ($DiscountResult->num_rows == 0) {
    echo json_encode(array("success" => false, "message" => "Discount code is not valid.", "discountPercent" => 0));
    exit();
}

$DiscountRow = $DiscountResult->fetch_assoc();

echo json_encode(array(
    "success" => true,
    "message" => "Discount code applied.",
    "discountPercent" => floatval($DiscountRow["discount_percent"])
));

$conn->close();
?>
