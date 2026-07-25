<?php
header("Content-Type: application/json");
require "db.php";

$SearchText = trim($_GET["search"] ?? "");
$SortType = $_GET["sort"] ?? "name";

$SortSql = "item_name ASC";

if ($SortType == "priceLow") {
    $SortSql = "price ASC";
} else if ($SortType == "priceHigh") {
    $SortSql = "price DESC";
} else if ($SortType == "available") {
    $SortSql = "quantity_available DESC";
}

$SearchValue = "%" . $SearchText . "%";
$ProductQuery = $conn->prepare(
    "SELECT product_id, item_name, item_description, price, quantity_available, image_url
     FROM products
     WHERE item_name LIKE ? OR item_description LIKE ?
     ORDER BY " . $SortSql
);
$ProductQuery->bind_param("ss", $SearchValue, $SearchValue);
$ProductQuery->execute();
$ProductResult = $ProductQuery->get_result();

$Products = array();

while ($ProductRow = $ProductResult->fetch_assoc()) {
    $Products[] = array(
        "productId" => intval($ProductRow["product_id"]),
        "itemName" => $ProductRow["item_name"],
        "itemDescription" => $ProductRow["item_description"],
        "price" => floatval($ProductRow["price"]),
        "quantityAvailable" => intval($ProductRow["quantity_available"]),
        "imageUrl" => $ProductRow["image_url"]
    );
}

echo json_encode(array("success" => true, "message" => "Products loaded.", "products" => $Products));
$conn->close();
?>
