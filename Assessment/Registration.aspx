<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Assessment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Form</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="Scripts/register.js"></script>
    <style>
        body, html {
            height: 100%;
            margin: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: #f0f2f5;
            font-family: 'Arial', sans-serif;
        }

        #txtform {
            padding: 30px;
            border: 2px solid #007bff;
            background-color: #ffffff;
            border-radius: 15px;
            box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.1);
            max-width: 600px;
            width: 100%;
        }

        h1 {
            font-size: 2rem;
            font-weight: bold;
            color: #007bff;
        }

        table {
            width: 100%;
            table-layout: fixed;
        }

        th {
            text-align: left;
            font-weight: bold;
            color: #333333;
        }

        td {
            padding: 8px 0;
        }

        .form-control {
            border-radius: 8px;
            border: 1px solid #ced4da;
        }

        .form-control:focus {
            border-color: #007bff;
            box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
        }

        .btn-primary {
            width: 100%;
            border-radius: 8px;
            padding: 10px;
        }

        .login-link {
            text-align: center;
            margin-top: 20px;
            font-size: 0.9rem;
        }

        .login-link a {
            color: #007bff;
            text-decoration: none;
        }

        .login-link a:hover {
            text-decoration: underline;
        }

        @media (max-width: 768px) {
            #txtform {
                padding: 20px;
                max-width: 90%;
            }

            th, td {
                font-size: 0.9rem;
            }

            .btn-primary {
                font-size: 1rem;
            }
        }
    </style>
</head>
<body>
    <div class="container">
        <div id="txtform">
            <h1 class="text-center">Registration Form</h1>
            <form id="form1" runat="server" onsubmit="Register(event)">
                <table>
                    <tr>
                        <th>First Name:<span class="text-danger">*</span></th>
                        <td><input id="txtFirstName" class="form-control" type="text" maxlength="100" name="First Name" placeholder="First Name" /></td>
                    </tr>
                    <tr class="hide">
                        <td class="text-danger p-2" id="firstNameError" colspan="2"></td>
                    </tr>
                    <tr>
                        <th>Last Name:<span class="text-danger">*</span></th>
                        <td><input id="txtLastName" class="form-control" type="text" maxlength="100" name="Last Name" placeholder="Last Name" /></td>
                    </tr>
                    <tr class="hide">
                        <td class="text-danger p-2" id="LastNameError" colspan="2"></td>
                    </tr>
                    <tr>
                        <th>Email id:<span class="text-danger">*</span></th>
                        <td><input id="txtEmailID" class="form-control" name="Email" maxlength="100" placeholder="Email Id" /></td>
                    </tr>
                    <tr class="hide">
                        <td class="text-danger p-2" id="EmailIdError" colspan="2"></td>
                    </tr>
                    <tr>
                        <th>Mobile No.:<span class="text-danger">*</span></th>
                        <td><input id="txtMobile" class="form-control" type="text" name="Mobile Number" maxlength="100" placeholder="Mobile Number" /></td>
                    </tr>
                    <tr class="hide">
                        <td class="text-danger p-2" id="MobileNoError" colspan="2"></td>
                    </tr>
                    <tr>
                        <th>Password:<span class="text-danger">*</span></th>
                        <td><input id="txtPassword" class="form-control" type="password" name="password" maxlength="100" placeholder="Password" /></td>
                    </tr>
                    <tr class="hide">
                        <td class="text-danger p-2" id="PasswordError" colspan="2"></td>
                    </tr>
                    <%--<tr id="AdminDiv">
                        <th>Make Admin:</th>
                        <td><input type="checkbox" id="makeAdmin" class="custom-checkbox cursor-pointer makeAdmin" name="makeAdmin" style="width: 20px; height: 20px;" /></td>
                    </tr>--%>
                     <tr id="RoleDiv">
                        <th>Role:<span class="text-danger">*</span></th>
                        <td>
                            <select id="roleDropdown" class="form-control" name="Role">
                                <option value="-1">Select Role</option>
                                <option value="Admin">Admin</option>
                                <option value="Trainee">Trainee</option>
                                <option value="Intern">Intern</option>
                            </select>
                        </td>
                    </tr>
                </table>
                <div class="button-group mt-3">
                    <button type="submit" id="btnSubmit" class="btn btn-primary" runat="server">Register</button>
                </div>
                <div class="login-link">
                    Already registered? <a href="Login.aspx">Login</a>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
