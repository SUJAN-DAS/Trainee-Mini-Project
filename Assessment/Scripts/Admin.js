jQuery(document).ready(function () {

    loadUserInfo();
});

function createUser() {
    // Logic to handle user creation
    alert("Redirecting to Create User page...");
    window.location.href ='/Registration.aspx'; // Adjust the path as needed
}

function logout() {
    // Logic for logout; redirect to login page
    alert("You are being logged out...");
    window.location.href = '/Login.aspx'; // Adjust the path as needed
}

function loadUserInfo() {
    jQuery.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/AdminData",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ userid: 0 }),
        dataType: "json",
        success: function (response) {
            console.log(response);
            if (response.d !== "0") {

                jQuery("#AdminInfotable").find('#tbdUser').append(response.d.HTMLDataList);
            } else {
                jQuery("#AdminInfotable tbody").html("<tr><td colspan='5'>No data found.</td></tr>");
            }

        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            console.error("Response Text: " + xhr.responseText);
        }
    });
}

function EditUserDetails(UserId) {
    window.location.href = "../Registration.aspx?Id=" + UserId;
}

function DeleteUserDetails(UserId) {
    if (confirm("Are you sure you want to delete this user?")) {
        deleteUser(UserId);
    }
}

//Update Role from admin Page to Database
function updateRole(dropdown) {
    var selectedRole = dropdown.value; // Get selected value
    var userId = dropdown.getAttribute('data-userid'); // Get user ID

    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/UpdateRole", // Web Service URL
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ userId: parseInt(userId), role: selectedRole }), // Data sent to server
        dataType: "json",
        success: function (response) {
            if (response.d) {
                alert("Role updated successfully!");
            } else {
                alert("Failed to update role.");
            }
        },
        error: function (xhr, status, error) {
            alert("An error occurred: " + error);
        }
    });
}
//Update Role from admin Page to Database


//delete
function deleteUser(UserId) {
    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/DeleteUser",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ userid: UserId }),
        dataType: "json",
        success: function (response) {
            if (response.d === "1") {
                alert("User deleted successfully.");
                jQuery("#tbdUser").empty();
                loadUserInfo()

            } else {
                alert("Failed to delete the user. Please try again.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            console.error("Response Text: " + xhr.responseText);
            
        }
    });
}

//Download and Upload

////#region Download Upload

//if (typeof languageResource === "undefined") {
//    var languageResource = {
//        resMsg_FailedToDownloadExcelFile: "Failed to download the Excel file.",
//        resMsg_Error: "Error: ",
//        resMsg_FailedToDoThisOperation: "Failed to perform the operation."
//    };

//function ShowUploadExcel() {
//    jQuery('#lblUploadError').html("");
//    jQuery("#divUploadExcel").slideToggle();
//}

//function UploadExcelFile(fileToUpload) {
//    jQuery("#lblUploadError").html("");
//    jQuery("#lblDownloadError").html("");
//    var isValid = false;
//    var isLabelDBOtherInfo = true;
//    var tempfile = jQuery("#" + fileToUpload).val().split('\\').pop();

//    var ext = tempfile.substring(tempfile.lastIndexOf('.') + 1).toLowerCase();
//    if (ext == "xlsx" || ext == "xls") {
//        isValid = true;
//    }
//    else {
//        jQuery("#lblUploadError").removeClass("text-info");
//        jQuery("#lblUploadError").addClass("text-danger");
//        jQuery("#lblUploadError").html(languageResource.resMsg_InvalidFormat);
//        isValid = false;
//    }
//    if (isValid == false) {
//        var control = jQuery("#" + fileToUpload);
//        control.replaceWith(control = control.val('').clone(true));
//    }

//    if (isValid) {
//        viewUserModal.ExcelDownloadInProgress(true);
//        jQuery.ajaxFileUpload({
//            type: "POST",
//            url: "UserInfoUploadHandler.ashx",
//            fileElementId: fileToUpload,
//            success: function (data, status) {
//                var tempJson = data.documentElement.innerText;
//                var tempSplit = tempJson.split(',');
//                var returnText = tempSplit[0];
//                if (returnText != "false") {
//                    var fileName = tempSplit[1];
//                    jQuery("#" + fileToUpload).attr("disabled", "disabled");
//                    timerMethodsForUploading(fileName, fileToUpload);
//                }
//                else {
//                    jQuery("#lblUploadError").removeClass("text-info");
//                    jQuery("#lblUploadError").addClass("text-danger");
//                    jQuery("#lblUploadError").html(tempSplit[1]);
//                    viewUserModal.ExcelDownloadInProgress(false);
//                    var control = jQuery("#" + fileToUpload);
//                    control.replaceWith(control = control.val('').clone(true));
//                }
//            },
//            error: function (data, status, e) {
//                jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
//                jQuery("#lblUploadError").removeClass("text-info");
//                jQuery("#lblUploadError").addClass("text-danger");
//                jQuery("#lblUploadError").html(languageResource.resMsg_FailedToUploadUserInfo);
//                viewUserModal.ExcelDownloadInProgress(false);
//                // jQuery("#divUploadProgress").hide();
//                var control = jQuery("#" + fileToUpload);
//                control.replaceWith(control = control.val('').clone(true));
//            }
//        });
//    }
//}

//function timerMethodsForUploading(fileName, fileToUpload) {
//    var btnDownloadUserInfo = fileToUpload.replace("uploadFile", "btnDownloadUserInfo");
//    var control = jQuery("#" + fileToUpload);
//    control.replaceWith(control = control.val('').clone(true));
//    jQuery("#" + btnDownloadUserInfo).attr("disabled", "disabled");
//    jQuery("#btnUploadUserInfo").attr("disabled", "disabled");

//    var successtimer;
//    var errortimer;
//    var successTextFile = fileName + "_tempSuccess.txt";
//    var errorTextFile = fileName + "_tempError.txt";
//    successtimer = setInterval(function () {
//        jQuery.ajax({
//            type: "POST",
//            url: jQuery.UserInfoNameSpace.ServicePath + "/ipas_PlantService.asmx/CheckFileExist",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify({ fileName: successTextFile }),
//            dataType: "json",
//            success: function (json) {
//                if (json.d != null) {
//                    if (json.d != false) {
//                        jQuery.get('../DownloadHandler.ashx?userInfoLogFile=' + successTextFile, function (data) {
//                            clearInterval(successtimer);
//                            clearInterval(errortimer);
//                            // jQuery("#divUploadProgress").hide();
//                            viewUserModal.ExcelDownloadInProgress(false);
//                            jQuery("#divUploadExcel").hide();
//                            jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
//                            jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
//                            jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
//                            jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
//                            window.location = "../DownloadHandler.ashx?userInfoLogFile=" + successTextFile;
//                        });
//                    }
//                }
//            },
//            error: function (request, error) {
//                clearInterval(successtimer);
//                clearInterval(errortimer);
//                ShowAjaxErrorMessage(request, error, languageResource.resMsg_FailedToDoThisOperation, false, "lblUploadError");
//                //jQuery("#divUploadProgress").hide();
//                viewUserModal.ExcelDownloadInProgress(false);
//                jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
//                jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
//                jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
//                jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
//            }
//        });

//    }, 20000);
//    errortimer = setInterval(function () {
//        jQuery.ajax({
//            type: "POST",
//            url: jQuery.UserInfoNameSpace.ServicePath + "/ipas_PlantService.asmx/CheckFileExist",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify({ fileName: errorTextFile }),
//            dataType: "json",
//            success: function (json) {
//                if (json.d != null) {
//                    if (json.d != false) {
//                        jQuery.get('../DownloadHandler.ashx?userInfoLogFile=' + errorTextFile, function (data) {
//                            clearInterval(successtimer);
//                            clearInterval(errortimer);
//                            jQuery("#lblUploadError").removeClass("text-info");
//                            jQuery("#lblUploadError").addClass("text-danger");
//                            jQuery("#lblUploadError").html(data);
//                            // jQuery("#divUploadProgress").hide();
//                            viewUserModal.ExcelDownloadInProgress(false);
//                            jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
//                            jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
//                            jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
//                        });
//                    }
//                }
//            },
//            error: function (request, error) {
//                clearInterval(successtimer);
//                clearInterval(errortimer);
//                ShowAjaxErrorMessage(request, error, languageResource.resMsg_FailedToDoThisOperation, false, "lblUploadError");
//                // jQuery("#divUploadProgress").hide();
//                viewUserModal.ExcelDownloadInProgress(false);
//                jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
//                jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
//                jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
//            }
//        });
//    }, 20000);
//}

//function DownloadUserInfoTemplate() {
//    jQuery("#lblDownloadError").html("");
//    jQuery("#lblUploadError").html("");
//    jQuery("#divUploadExcel").hide();

//    jQuery.ajax({
//        type: "POST",
//        url:"WebService/UserRegistration.asmx/DownloadUserInfoTemplate",
//        data: JSON.stringify({}),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (json) {
//            if (json.d !== null) {
//                if (json.d !== "") {
//                    timerMethodsForDownloading(json.d);
//                }
//                else {
//                    jQuery("#lblDownloadError").addClass("text-danger");
//                    jQuery("#lblDownloadError").html(languageResource.resMsg_FailedToDownloadUserInfo);
//                }
//            }
//            else {
//                jQuery("#lblDownloadError").addClass("text-danger");
//                jQuery("#lblDownloadError").html(languageResource.resMsg_FailedToDownloadUserInfo);
//            }
//        },
//        beforeSend: function () {
//            jQuery("#btnDownloadTemplate").attr("disabled", "disabled");

//        },
//        complete: function () {
//        },
//        error: function (request, error) {
//            ShowAjaxErrorMessage(request, error, languageResource.resMsg_FailedToDoThisOperation, false, "lblDownloadError");
            
//        }
//    });
//}

//function DownloadAllUserInfo() {
//    jQuery("#lblDownloadError").html("");
//    jQuery("#lblUploadError").html("");
//    jQuery("#divUploadExcel").hide();
//    var btnDownloadUserInfo = jQuery.UserInfoNameSpace.PagerData.LoadControlID.replace("divUserInformationDisplay", "btnDownloadUserInfo");


//    var departmentRoleIDList = jQuery("#divDepartmentAndRolesData").val();

//    var featureInfo = ko.utils.arrayFirst(jQuery.DynamicPageNamespace.PageFeatureList, function (item) {
//        return jQuery.UserInfoNameSpace.FeatureList.PLANT_USER_INFORMATION_LIST_FILTER == item.FeatureID;
//    });

//    if (featureInfo.IsSearch) {
//        var returnParameterInfo = GetFilterInfoForFeature(jQuery.UserInfoNameSpace.FeatureList.PLANT_USER_INFORMATION_LIST_FILTER);
//        if (returnParameterInfo.IsValid) {
//            if ((returnParameterInfo.USERNAME != undefined && returnParameterInfo.USERNAME.length > 0)
//                || (returnParameterInfo.EMPLOYEEID != undefined && returnParameterInfo.EMPLOYEEID.length > 0)
//                || (returnParameterInfo.EMAILID != undefined && returnParameterInfo.EMAILID.length > 0)
//                || departmentRoleIDList != undefined && departmentRoleIDList.length > 0) {
//                filterInfo.SearchName = returnParameterInfo.USERNAME;
//                filterInfo.SearchEmployeeID = returnParameterInfo.EMPLOYEEID;
//                if (returnParameterInfo.EMAILID != undefined)
//                    filterInfo.SearchEmail = returnParameterInfo.EMAILID;

//                if (departmentRoleIDList != undefined) {
//                    jQuery.each(departmentRoleIDList, function (index, data) {
//                        filterInfo.DepartmentRoleIDList.push(parseInt(data));
//                    });
//                }
//                filterInfo.USERTYPE = returnParameterInfo.USERTYPE;
//            }
//        }
//    }

//    if (viewUserModal.UserStatusCountList().length > 0) {
//        var statusAllSelected = ko.utils.arrayFirst(viewUserModal.UserStatusCountList(), function (item) {
//            return (jQuery.UserInfoNameSpace.UserStatus.All.charCodeAt(0) == item.UserStatus && item.IsSelected());
//        });

//        if (statusAllSelected == null) {
//            ko.utils.arrayForEach(viewUserModal.UserStatusCountList(), function (item) {
//                if (item.IsSelected()) {
//                    if (item.UserStatus == jQuery.UserInfoNameSpace.UserStatus.Locked.charCodeAt(0)) {
//                        filterInfo.UserStatusList.push(jQuery.UserInfoNameSpace.UserStatus.Locked);
//                    }
//                    else if (item.UserStatus == jQuery.UserInfoNameSpace.UserStatus.Inactive.charCodeAt(0)) {
//                        filterInfo.UserStatusList.push(jQuery.UserInfoNameSpace.UserStatus.Inactive);
//                    }
//                    else if (item.UserStatus == jQuery.UserInfoNameSpace.UserStatus.Active.charCodeAt(0)) {
//                        filterInfo.UserStatusList.push(jQuery.UserInfoNameSpace.UserStatus.Active);
//                    }
//                }
//            });

//            if (viewUserModal.UserStatusCountList().length - 1 == filterInfo.UserStatusList) {
//                filterInfo.UserStatusList = []; // All 3 options are selected
//            }
//        }
//    }

//    jQuery.ajax({
//        type: "POST",
//        url: jQuery.UserInfoNameSpace.ServicePath + "/ipas_PlantService.asmx/DownloadAllUserDetails",
//        data: JSON.stringify({ userFilter: filterInfo }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (json) {
//            if (json.d != null && json.d != "") {
//                timerMethodsForDownloading(json.d);
//            }
//            else {
//                jQuery("#lblDownloadError").addClass("text-danger");
//                jQuery("#lblDownloadError").html(languageResource.resMsg_FailedToDownloadUserInfo);
//            }
//        },
//        beforeSend: function () {
//            jQuery("#" + btnDownloadUserInfo).attr("disabled", "disabled");
//            jQuery("#btnUploadUserInfo").attr("disabled", "disabled");
//            viewUserModal.ExcelDownloadInProgress(true);
//        },
//        complete: function () {
//        },
//        error: function (request, error) {
//            ShowAjaxErrorMessage(request, error, languageResource.resMsg_FailedToDoThisOperation, false, "lblDownloadError");
//            jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
//            viewUserModal.ExcelDownloadInProgress(false);

//            if (jQuery.UserInfoNameSpace.PageAccessRights == "FULL_ACCESS")
//                jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
//        }
//    });
//}

//function timerMethodsForDownloading(jsonValue) {
//    var successtimer;
//    var errortimer;
//    var successTextFile = jsonValue + "_tempSuccess.txt";
//    var errorTextFile = jsonValue + "_tempError.txt";
//    var excelFileName = jsonValue + ".xlsx";
//    successtimer = setInterval(function () {
//        jQuery.ajax({
//            type: "POST",
//            url: "WebService/UserRegistration.asmx/CheckFileExist",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify({ fileName: successTextFile }),
//            dataType: "json",
//            success: function (json) {
//                if (json.d != null) {
//                    if (json.d != false) {
//                        clearInterval(successtimer);
//                        clearInterval(errortimer);
//                        jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
//                        window.location = "../DownloadHandler.ashx?userInfoExcel=" + excelFileName;
//                    }
//                }
//            },
//            error: function (request, error) {
//                clearInterval(successtimer);
//                clearInterval(errortimer);
//                ShowAjaxErrorMessage(request, error, languageResource.resMsg_FailedToDoThisOperation, false, "lblDownloadError");
                
//                jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                
//            }
//        });

//    }, 10000);
//    errortimer = setInterval(function () {
//        jQuery.ajax({
//            type: "POST",
//            url: "WebService/UserRegistration.asmx/CheckFileExist",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify({ fileName: errorTextFile }),
//            dataType: "json",
//            success: function (json) {
//                if (json.d != null) {
//                    if (json.d != false) {
//                        clearInterval(successtimer);
//                        clearInterval(errortimer);
//                        jQuery("#lblDownloadError").html(languageResource.resMsg_FailedToDownloadUserInfo);
                       
//                        jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                        
//                    }
//                }
//            },
//            error: function (request, error) {
//                clearInterval(successtimer);
//                clearInterval(errortimer);
//                ShowAjaxErrorMessage(request, error, languageResource.resMsg_FailedToDoThisOperation, false, "lblDownloadError");
//                jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                
//            }
//        });
//    }, 10000);
//}

////#endregion