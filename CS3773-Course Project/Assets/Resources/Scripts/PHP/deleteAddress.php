<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_POST["userId"] ?? 0);
$AddressId = intval($_POST["addressId"] ?? 0);

if ($UserId <= 0 || $AddressId <= 0) {
    echo json_encode(array("success" => false, "message" => "User and Address are required."));
    exit();
}

$DeleteQuery = $conn->prepare("DELETE FROM addresses WHERE address_id = ? AND user_id = ?");
$DeleteQuery->bind_param("ii", $AddressId, $UserId);

if ($DeleteQuery->execute()) {
    if ($DeleteQuery->affected_rows > 0) {
        echo json_encode(array("success" => true, "message" => "Address deleted successfully."));
    } else {
        echo json_encode(array("success" => false, "message" => "Address not found or already deleted."));
    }
} else {
    echo json_encode(array("success" => false, "message" => "Failed to delete address: " . $conn->error));
}

$DeleteQuery->close();
$conn->close();
?>