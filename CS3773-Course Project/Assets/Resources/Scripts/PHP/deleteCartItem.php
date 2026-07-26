<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_POST["userId"] ?? 0);
$CartItemId = intval($_POST["cartItemId"] ?? 0);

$DeleteQuery = $conn->prepare("DELETE FROM cart_items WHERE cart_item_id = ? AND user_id = ?");
$DeleteQuery->bind_param("ii", $CartItemId, $UserId);
$DeleteQuery->execute();

echo json_encode(array(
    "success" => $DeleteQuery->affected_rows > 0,
    "message" => $DeleteQuery->affected_rows > 0 ? "Item deleted." : "Item was not found."
));

$conn->close();
?>
