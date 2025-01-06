<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Assessment.Admin" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Page</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet"/>
     <script src="Scripts/jquery-3.5.1.min.js"></script>
<script src="Scripts/ajaxfileupload.js"></script>

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
       
        .centered-text {
        display: flex;
        justify-content: center;
        align-items: center;
        margin: 20px 0; 
    }
    .centered-text span {
        font-size: 1.5em; 
        font-weight: bold; 
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
     <form id="form1" runat="server" class="form-container">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <div class="row">
        <div class="col-xs-12 col-md-6 table-with-heading">

            <!-- Logout Button -->
            <div class="text-right">
                <button id="btnLogout" type="button" class="btn btn-danger mb-3" onclick="Logout();">Logout</button>

            </div>
           <%-- logout--%>

           <%-- for download and upload--%>
            
            
            <section class="search-section" style="min-height: 40px;margin-bottom:0px;">
                <div class="down-up-div mr-3" style="padding-top: 10px;">
                        <div class="download_upload mr-2"></div>
                        <div>Download & Upload</div>
                    </div>
                    <fieldset class="down-up-block hide" id="downUp-block">
                        <fieldset class="row mx-0 py-2" style="padding-left:9px;">
                            <a id="btnDownloadTemplate" type="button" runat="server" class="btn btn-outline-light btn-sm float-left" onclick="DownloadUserInfoTemplate(); return false;">
                                <i class="float-left i-download mr-1 float-left"></i><span>Download Template</span>
                            </a>

                            <button id="btnUploadUserInfo" type="button" class="btn btn-outline-light btn-sm float-left" onclick="ShowUploadExcel(); return false;">
                                <i class="float-left i-upload mr-1 float-left"></i>
                                <span>Upload User Info</span>
                            </button>

<%--                            <button id="btnDownloadUserInfo" type="button" class="btn btn-outline-light btn-sm float-left" runat="server" onclick="DownloadAllLogFail(); return false;">
                                <i class="float-left i-download mr-1 float-left"></i><span>Download Log Failed</span>
                            </button>--%>

                            <span class="spinner-circle-small float-left" style="border-width: 3px; border-style: dotted; border-color: rgb(36, 138, 204) rgb(246, 246, 246) rgb(246, 246, 246);"></span>
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

                </section>
            <%-- for download and upload--%>
            <div>
                <span class="centered-text" id="lblAdminName"> Admin </span>
            </div>
        


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
            
            <div class="text-center mt-3">
           
                <button id="btnCreateUser" type="button" class="btn btn-primary" onclick="CreateUser()">Create User</button>

        </div>
           
        </div>
    </div>
        </form>
</body>
</html>