<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Assessment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css"/>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="Scripts/register.js"></script>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            background: white;
            padding: 20px;
            width: 300px;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }
        .form-container h1 {
            text-align: center;
            margin-bottom: 20px;
            color: #333;
        }
        .form-container input[type="text"],
        .form-container input[type="password"],
        .form-container input[type="submit"] {
            width: 100%;
            padding: 10px;
            margin: 10px 0;
            border-radius: 5px;
            border: 1px solid #ddd;
            box-sizing: border-box;
        }
        .form-container input[type="submit"] {
            background-color: #007bff;
            color: white;
            border: none;
            cursor: pointer;
        }
        .form-container input[type="submit"]:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="form-container">
            <form id="form1" runat="server" class="form-container" onsubmit="Login(event)">
                <div class="login-container">
                    <h1>Login</h1>
                    <input id="txtEmail" type="text" placeholder="Email Id"/>
                    <input id="txtPassword" type="password" placeholder="Password"/>
                    <input id="btnlogin" type="submit" value="Login" />
                    <div class="register-link">
                        Not yet registered? <a href="Registration.aspx">Register here</a>
                    </div>
                </div>
            </form>
        </div>

    </div>
</body>
</html>
