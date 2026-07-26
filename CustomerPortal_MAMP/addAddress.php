<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_POST["userId"] ?? 0);
$AddressName = trim($_POST["addressName"] ?? "");
$Street = trim($_POST["street"] ?? "");
$City = trim($_POST["city"] ?? "");
$State = trim($_POST["state"] ?? "");
$ZipCode = trim($_POST["zipCode"] ?? "");

if ($UserId <= 0 || $AddressName == "" || $Street == "" || $City == "" || $State == "" || $ZipCode == "") {
    echo json_encode(array("success" => false, "message" => "All address fields are required."));
    exit();
}

$AddressQuery = $conn->prepare(
    "INSERT INTO addresses (user_id, address_name, street, city, state, zip_code)
     VALUES (?, ?, ?, ?, ?, ?)"
);
$AddressQuery->bind_param("isssss", $UserId, $AddressName, $Street, $City, $State, $ZipCode);

if ($AddressQuery->execute()) {
    echo json_encode(array("success" => true, "message" => "Address added.", "addressId" => $conn->insert_id));
} else {
    echo json_encode(array("success" => false, "message" => "Address could not be added."));
}

$conn->close();
?>
