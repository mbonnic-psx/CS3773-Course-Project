<?php
header("Content-Type: application/json");
require "db.php";

$UserEmail = trim($_POST["email"] ?? "");
$Username = trim($_POST["username"] ?? "");
$UserPassword = $_POST["password"] ?? "";

if ($UserEmail == "" || $Username == "" || $UserPassword == "") {
    echo json_encode(array("success" => false, "message" => "All fields are required."));
    exit();
}

if (!filter_var($UserEmail, FILTER_VALIDATE_EMAIL)) {
    echo json_encode(array("success" => false, "message" => "Email is not valid."));
    exit();
}

$CheckUserQuery = $conn->prepare("SELECT user_id FROM users WHERE email = ? OR username = ?");
$CheckUserQuery->bind_param("ss", $UserEmail, $Username);
$CheckUserQuery->execute();
$CheckUserResult = $CheckUserQuery->get_result();

if ($CheckUserResult->num_rows > 0) {
    echo json_encode(array("success" => false, "message" => "Email or username already exists."));
    exit();
}

$UserPasswordHash = password_hash($UserPassword, PASSWORD_DEFAULT);

$RegisterUserQuery = $conn->prepare("INSERT INTO users (email, username, password) VALUES (?, ?, ?)");
$RegisterUserQuery->bind_param("sss", $UserEmail, $Username, $UserPasswordHash);

if ($RegisterUserQuery->execute()) {
    echo json_encode(array(
        "success" => true,
        "message" => "Account created.",
        "userId" => $conn->insert_id,
        "username" => $Username
    ));
} else {
    echo json_encode(array("success" => false, "message" => "Account could not be created."));
}

$conn->close();
?>
