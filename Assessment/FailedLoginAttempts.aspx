<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FailedLoginAttempts.aspx.cs" Inherits="Assessment.FailedLoginAttempts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Failed Login Attempts</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="Scripts/FailedLogin.js"></script>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
        }
        h1 {
            color: #333;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        th, td {
            border: 1px solid #ccc;
            padding: 10px;
            text-align: left;
        }
        th {
            background-color: #f4f4f4;
        }
        input[type="text"] {
            padding: 5px;
            width: 300px;
            margin-top: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Failed Login Attempts</h1>

        <!-- Search by Username -->
        <label for="searchUsername">Search by Username:</label>
        <input type="text" id="searchUsername" name="searchUsername" placeholder="Enter username to search" />
        <button type="button" onclick="searchLogs(event)">Search</button>
        <!-- Table to display failed login attempts -->
        <table id="failedLoginTable">
            <thead>
                <tr>
                    <th>Username</th>
                    <th>Password</th>
                    <th>Most Recent Timestamp</th>
                    <th>Number of Failed Attempts</th>
                </tr>
            </thead>
            <tbody id="tbdlog">
                <!-- Data rows will be populated here -->
            </tbody>
        </table>
    </form>
</body>
</html>
