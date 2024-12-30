<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="Assessment.User" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Page</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css"/>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="Scripts/User.js"></script>
    <style>
        body, html {
            height: 100%;
            margin: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: #f5f5f5;
        }

        #txtform {
            padding: 20px;
            border: 1px solid #ccc;
            background-color: #ffffff;
            margin: 15px;
            width: 100%;
            max-width: 600px;
            border-radius: 10px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
        }

        h1 {
            font-size: 1.8rem;
            margin-bottom: 20px;
            text-align: center;
            font-family: 'Arial', sans-serif;
            color: #333;
        }

        table {
            width: 100%;
            table-layout: auto;
        }

        th {
            text-align: left;
            font-weight: bold;
            padding: 8px 10px;
            vertical-align: middle;
        }

        td {
            padding: 8px 10px;
        }

        .btn-primary {
            background-color: #5cb85c;
            border-color: #4cae4c;
            color: #fff;
            padding: 10px 15px;
            font-size: 1rem;
            border-radius: 5px;
        }

        .btn-primary:hover {
            background-color: #4cae4c;
            border-color: #45a045;
        }

        .text-danger {
            color: #d9534f !important;
        }

        .hide {
            display: none;
        }

        @media (max-width: 768px) {
            #txtform {
                padding: 15px;
                width: 95%;
            }

            th, td {
                font-size: 0.9rem;
                padding: 6px;
            }

            h1 {
                font-size: 1.5rem;
            }

            .btn-primary {
                width: 100%;
                padding: 12px;
            }
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="form-container">
            <form id="userForm" runat="server" class="form-container">
                <div id="txtform">
                    <h1>User Form</h1>
                    <table class="table table-bordered">
                        <tr>
                            <th>City :<span class="text-danger">*</span></th>
                            <td>
                                <input id="txtCity" class="form-control" type="text" maxlength="100" name="City" placeholder="City" />
                            </td>
                        </tr>
                        <tr class="hide">
                            <td class="text-danger p-2" id="cityError" colspan="2"></td>
                        </tr>
                        <tr>
                            <th>State : <span class="text-danger">*</span></th>
                            <td>
                                <input id="txtState" class="form-control" type="text" maxlength="100" name="State" placeholder="State" />
                            </td>
                        </tr>
                        <tr class="hide">
                            <td class="text-danger p-2" id="StateError" colspan="2"></td>
                        </tr>
                        <tr>
                            <th>Country</th>
                            <td>
                                <select id="ddlCountry" class="form-control" name="Country">
                                    <option value="-1" selected="selected">Select Country</option>
                                    <option value="India">India</option>
                                    <option value="United States">United States</option>
                                    <option value="United Kingdom">United Kingdom</option>
                                    <option value="Canada">Canada</option>
                                    <option value="Australia">Australia</option>
                                    <option value="Germany">Germany</option>
                                    <option value="France">France</option>
                                    <option value="Japan">Japan</option>
                                    <option value="China">China</option>
                                    <option value="Brazil">Brazil</option>
                                </select>
                            </td>
                        </tr>
                        <tr class="hide">
                            <td class="text-danger p-2" id="CountryError" colspan="2"></td>
                        </tr>
                        <tr>
                            <th>Gender<span class="text-danger">*</span></th>
                            <td>
                                <label><input type="radio" name="gender" value="male" /> Male</label>
                                <label><input type="radio" name="gender" value="female" /> Female</label>
                                <label><input type="radio" name="gender" value="other" /> Other</label>
                            </td>
                        </tr>
                        <tr class="hide">
                            <td class="text-danger p-2" id="GenderError" colspan="2"></td>
                        </tr>
                        <tr>
                            <th>Date-Of-Birth <span class="text-danger">*</span></th>
                            <td>
                                <input id="txtDob" class="form-control" type="date" name="DOB" maxlength="100" placeholder="DOB" />
                            </td>
                        </tr>
                        <tr class="hide">
                            <td class="text-danger p-2" id="DobError" colspan="2"></td>
                        </tr>
                        <tr>
                            <th>Profile Image<span class="text-danger">*</span></th>
                            <td>
                                <input id="txtImage" class="form-control" type="file" accept="image/*"/>
                            </td>
                        </tr>
                        <tr class="hide">
                            <td class="text-danger p-2" id="ProfileError" colspan="2"></td>
                        </tr>
                    </table>
                    <div class="button-group text-center">
                        <button type="submit" id="btnSubmit" class="btn btn-primary">Submit</button>
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
