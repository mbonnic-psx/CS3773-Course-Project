<?php
$servername = "localhost";
$DbUsername = "root";
$DbPassword = "root";
$dbname = "CustomerPortal";

$conn = new mysqli($servername, $DbUsername, $DbPassword, $dbname);

if ($conn->connect_error) {
    echo json_encode(array(
        "success" => false,
        "message" => "Database connection failed."
    ));
    exit();
}

$conn->set_charset("utf8mb4");
?>
