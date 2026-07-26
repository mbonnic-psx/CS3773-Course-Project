<?php
header("Content-Type: application/json");
require "db.php";

$UserId = intval($_GET["userId"] ?? 0);

$AddressQuery = $conn->prepare(
    "SELECT address_id, address_name, street, city, state, zip_code
     FROM addresses WHERE user_id = ? ORDER BY address_id DESC"
);
$AddressQuery->bind_param("i", $UserId);
$AddressQuery->execute();
$AddressResult = $AddressQuery->get_result();

$Addresses = array();

while ($AddressRow = $AddressResult->fetch_assoc()) {
    $Addresses[] = array(
        "addressId" => intval($AddressRow["address_id"]),
        "addressName" => $AddressRow["address_name"],
        "street" => $AddressRow["street"],
        "city" => $AddressRow["city"],
        "state" => $AddressRow["state"],
        "zipCode" => $AddressRow["zip_code"]
    );
}

echo json_encode(array("success" => true, "message" => "Addresses loaded.", "addresses" => $Addresses));
$conn->close();
?>
