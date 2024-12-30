<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Assessment.Admin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Page</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet"/>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script type="text/javascript" src="/Scripts/Admin.js"></script> 
    <style>
        body {
            background-color: #f8f9fa;
            font-family: Arial, sans-serif;
            padding: 20px;
        }

        .table-with-heading {
            background-color: #ffffff;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            padding: 20px;
            margin: 20px auto;
            max-width: 900px;
        }

        .table-with-heading span {
            font-size: 24px;
            font-weight: bold;
            color: #343a40;
        }

        .scroll-x {
            overflow-x: auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            padding: 10px;
            text-align: center;
            border: 1px solid #ddd;
        }
       /* Admin above*/
        .centered-text {
        display: flex;
        justify-content: center;
        align-items: center;
        margin: 20px 0; /* Optional for spacing */
    }
    .centered-text span {
        font-size: 1.5em; /* Optional for larger text */
        font-weight: bold; /* Optional for styling */
    }

        th {
            background-color: #f1f1f1;
            color: #343a40;
            text-transform: uppercase;
            font-size: 14px;
            cursor: pointer;
        }

        th u {
            text-decoration: none;
            color: #007bff;
        }

        th:hover u {
            text-decoration: underline;
        }

        tbody tr:nth-child(odd) {
            background-color: #f9f9f9;
        }

        tbody tr:nth-child(even) {
            background-color: #ffffff;
        }

        tbody tr:hover {
            background-color: #f1f1f1;
            cursor: pointer;
        }

        .action-buttons button {
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 3px;
            padding: 5px 10px;
            font-size: 14px;
        }

        .action-buttons button:hover {
            background-color: #0056b3;
        }

        @media (max-width: 768px) {
            .table-with-heading {
                padding: 10px;
            }

            th, td {
                font-size: 12px;
            }

            .action-buttons button {
                font-size: 12px;
                padding: 3px 8px;
            }
        }
    </style>
</head>
<body>
    <div class="row">
        <div class="col-xs-12 col-md-6 table-with-heading">

            <!-- Logout Button -->
            <div class="text-right">
                <button id="btnLogout" class="btn btn-danger mb-3" onclick="logout()">Logout</button>
            </div>
           <%-- logout--%>

           <%-- for download and upload--%>

            
            <%--<section class="search-section" style="min-height: 40px;margin-bottom:0px;">
                <div class="down-up-div mr-3" style="padding-top: 10px;">
                        <div class="download_upload mr-2"></div>
                        <div>Download & Upload</div>
                    </div>
                    <fieldset class="down-up-block hide" id="downUp-block">
                        <fieldset class="row mx-0 py-2" style="padding-left:9px;">
                            <a id="btnDownloadTemplate" type="button" runat="server" class="btn btn-outline-light btn-sm float-left" data-bind="enable: !viewUserModal.ExcelDownloadInProgress()" onclick="DownloadUserInfoTemplate(); return false;">
                                <i class="float-left i-download mr-1 float-left"></i><span>Download Template</span>
                            </a>

                            <button id="btnUploadUserInfo" type="button" class="btn btn-outline-light btn-sm float-left" onclick="ShowUploadExcel(); return false;">
                                <i class="float-left i-upload mr-1 float-left"></i>
                                <span>Upload User Info</span>
                            </button>

                            <button id="btnDownloadUserInfo" type="button" class="btn btn-outline-light btn-sm float-left" runat="server" data-bind="enable: !viewUserModal.ExcelDownloadInProgress()" onclick="DownloadAllUserInfo(); return false;">
                                <i class="float-left i-download mr-1 float-left"></i><span>Download User Info</span>
                            </button>

                            <span data-bind="css: { '': viewUserModal.ExcelDownloadInProgress(), 'hide': !viewUserModal.ExcelDownloadInProgress() }" class="spinner-circle-small float-left" style="border-width: 3px; border-style: dotted; border-color: rgb(36, 138, 204) rgb(246, 246, 246) rgb(246, 246, 246);"></span>
                            <span class="text-danger clearfix"></span>

                            <div id="divUploadExcel" class="col-12 p-0 push-down" style="display: none">
                                <div id="divFileExcel">
                                    <input name="fileToUpload" type="file" id="uploadFile" class="form-control input ml-1 " runat="server" />
                                </div>
                                <p id="lblUploadError" class="text-danger col-12 mb-0 float-left pl-0 mt-1"></p>
                            </div>
                            <div class="text-danger col-xs-12 font-small push-up" id="lblDownloadError">
                            </div>

                        </fieldset>
                    </fieldset>

                </section>--%>
            <%-- for download and upload--%>
            <div>
                <span class="centered-text"> Admin </span>
            </div>
        <%--    <span class="centered-text">
    <asp:Label ID="lblAdminName" runat="server" Text="Admin" />
</span>--%>


            <div class="scroll-x">
                <table id="AdminInfotable">
                    <thead>
                        <tr>
                            <th class="bg-graylight cursor-pointer text-xs-center" id="thFName" style="width: 120px;">
                                <u>FName</u><i class="tiny-leftmargin cursor-pointer"></i>
                            </th>
                            <th class="bg-graylight cursor-pointer text-xs-center" id="LName" style="width: 120px;">
                                <u>LName</u><i class="tiny-leftmargin cursor-pointer"></i>
                            </th>
                            <th class="bg-graylight cursor-pointer text-xs-center" id="thEmail" style="width: 120px;">
                                <u>Email</u><i class="tiny-leftmargin cursor-pointer"></i>
                            </th>
                            <th class="bg-graylight cursor-pointer text-xs-center" id="thRoleID" style="width: 120px;">
                                <u>Role</u><i class="tiny-leftmargin cursor-pointer"></i>
                            </th>
                            <th class="bg-graylight text-xs-center" style="width: 120px;">Action</th>
                        </tr>
                    </thead>
                    <tbody id="tbdUser">
                      </tbody>
                </table>
            </div>
            <%--//Create User button--%>
            <div class="text-center mt-3">
            <button id="btnCreateUser" class="btn btn-primary" onclick="createUser()">Create User</button>
        </div>
            <%--Create user--%>
        </div>
    </div>
</body>
</html>