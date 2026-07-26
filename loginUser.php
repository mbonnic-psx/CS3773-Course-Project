<?php
header("Content-Type: application/json");
require "db.php";

$LoginName = trim($_POST["loginName"] ?? "");
$UserPassword = $_POST["password"] ?? "";

if ($LoginName == "" || $UserPassword == "") {
    echo json_encode(array("success" => false, "message" => "Login and password are required."));
    exit();
}

$LoginUserQuery = $conn->prepare("SELECT user_id, email, username, password FROM users WHERE email = ? OR username = ?");
$LoginUserQuery->bind_param("ss", $LoginName, $LoginName);
$LoginUserQuery->execute();
$LoginUserResult = $LoginUserQuery->get_result();

if ($LoginUserResult->num_rows == 0) {
    echo json_encode(array("success" => false, "message" => "Account was not found."));
    exit();
}

$UserRow = $LoginUserResult->fetch_assoc();

if (!password_verify($UserPassword, $UserRow["password"])) {
    echo json_encode(array("success" => false, "message" => "Password is incorrect."));
    exit();
}

echo json_encode(array(
    "success" => true,
    "message" => "Login successful.",
    "userId" => intval($UserRow["user_id"]),
    "email" => $UserRow["email"],
    "username" => $UserRow["username"]
));

$conn->close();
?>
