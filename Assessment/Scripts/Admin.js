jQuery(document).ready(function () {

    LoadUserInfo();
});

function CreateUser() {
    alert("Redirecting to Create User page...");
    window.location.href ='/Registration.aspx';
}

function Logout() {
    alert("You are being logged out...");
    window.location.href = '/Login.aspx';
}

function LoadUserInfo() {
    jQuery.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/GetAdminData",
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
        DeleteUser(UserId);
    }
}

//Update Role from admin Page to Database
function UpdateRole(dropdown) {
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
function DeleteUser(UserId) {
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
                LoadUserInfo()

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


function ShowUploadExcel() {
    jQuery('#lblUploadError').html("");
    jQuery("#divUploadExcel").slideToggle();
}

function UploadExcelFile(fileToUpload) {
    jQuery("#lblUploadError").html("");
    jQuery("#lblDownloadError").html("");
    var isValid = false;
    var isLabelDBOtherInfo = true;
    var tempfile = jQuery("#" + fileToUpload).val().split('\\').pop();

    var ext = tempfile.substring(tempfile.lastIndexOf('.') + 1).toLowerCase();
    if (ext == "xlsx" || ext == "xls") {
        isValid = true;
    }
    else {
        jQuery("#lblUploadError").removeClass("text-info");
        jQuery("#lblUploadError").addClass("text-danger");
        jQuery("#lblUploadError").html(languageResource.resMsg_InvalidFormat);
        isValid = false;
    }
    if (isValid == false) {
        var control = jQuery("#" + fileToUpload);
        control.replaceWith(control = control.val('').clone(true));
    }

    if (isValid) {
        jQuery.ajaxFileUpload({
            type: "POST",
            url: "/HandlerFiles/UserInfoUploadHandler.ashx",
            fileElementId: fileToUpload,
            success: function (data, status) {
                var tempJson = data.documentElement.innerText;
                var tempSplit = tempJson.split(',');
                var returnText = tempSplit[0];
                if (returnText != "false") {
                    var fileName = tempSplit[1];
                    jQuery("#" + fileToUpload).attr("disabled", "disabled");
                    TimerMethodsForUploading(fileName, fileToUpload);
                    
                    
                }
                else {
                    jQuery("#lblUploadError").removeClass("text-info");
                    jQuery("#lblUploadError").addClass("text-danger");
                    jQuery("#lblUploadError").html(tempSplit[1]);
                    var control = jQuery("#" + fileToUpload);
                    control.replaceWith(control = control.val('').clone(true));
                }
            },
            error: function (data, status, e) {
                jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
                jQuery("#lblUploadError").removeClass("text-info");
                jQuery("#lblUploadError").addClass("text-danger");
                jQuery("#lblUploadError").html(languageResource.resMsg_FailedToUploadUserInfo);
                
                var control = jQuery("#" + fileToUpload);
                control.replaceWith(control = control.val('').clone(true));
            }
        });
    }
}
function TimerMethodsForUploading(fileName, fileToUpload) {
    var btnDownloadUserInfo = fileToUpload.replace("uploadFile", "btnDownloadUserInfo");
    var control = jQuery("#" + fileToUpload);
    control.replaceWith(control = control.val('').clone(true));
    jQuery("#" + btnDownloadUserInfo).attr("disabled", "disabled");
    jQuery("#btnUploadUserInfo").attr("disabled", "disabled");

    var successtimer;
    var errortimer;
    var successTextFile = fileName + "_tempSuccess.txt";
    var errorTextFile = fileName + "_tempError.txt";
    successtimer = setInterval(function () {
        jQuery.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/CheckFileExist",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ fileName: successTextFile }),
            dataType: "json",
            success: function (json) {
                if (json.d != null) {
                    if (json.d != false) {
                        jQuery.get('../HandlerFiles/DownloadHandler.ashx?userInfoLogFile=' + successTextFile, function (data) {
                            clearInterval(successtimer);
                            clearInterval(errortimer);
                             jQuery("#divUploadProgress").hide();
                            jQuery("#divUploadExcel").hide();
                            jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
                            jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
                            jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
                            jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                            
                            window.location = "../HandlerFiles/DownloadHandler.ashx?userInfoLogFile=" + successTextFile;
                            jQuery("#AdminInfotable").find('#tbdUser').empty();
                            LoadUserInfo();
                            
                        });
                    }
                }
            },
            error: function (request, error) {
                clearInterval(successtimer);
                clearInterval(errortimer);
                jQuery("#divUploadProgress").hide();
                jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
                jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
                jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
                jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
            }
        });

    }, 20000);
    errortimer = setInterval(function () {
        jQuery.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/CheckFileExist",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ fileName: errorTextFile }),
            dataType: "json",
            success: function (json) {
                if (json.d != null) {
                    if (json.d != false) {
                        jQuery.get('../DownloadHandler.ashx?userInfoLogFile=' + errorTextFile, function (data) {
                            clearInterval(successtimer);
                            clearInterval(errortimer);
                            jQuery("#lblUploadError").removeClass("text-info");
                            jQuery("#lblUploadError").addClass("text-danger");
                            jQuery("#lblUploadError").html(data);
                             jQuery("#divUploadProgress").hide();
                            jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
                            jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
                            jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
                        });
                    }
                }
            },
            error: function (request, error) {
                clearInterval(successtimer);
                clearInterval(errortimer);
                 jQuery("#divUploadProgress").hide();
                jQuery("#" + fileToUpload).removeAttr("disabled", "disabled");
                jQuery("#btnUploadUserInfo").removeAttr("disabled", "disabled");
                jQuery("#" + btnDownloadUserInfo).removeAttr("disabled", "disabled");
            }
        });
    }, 20000);
}

function DownloadUserInfoTemplate() {
    jQuery("#lblDownloadError").html("");
    jQuery("#lblUploadError").html("");
    jQuery("#divUploadExcel").hide();

    jQuery.ajax({
        type: "POST",
        url:"WebService/UserRegistration.asmx/DownloadUserInfoTemplate",
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            if (json.d !== null) {
                if (json.d !== "") {
                    TimerMethodsForDownloading(json.d);
                }
                else {
                    jQuery("#lblDownloadError").addClass("text-danger");
                    jQuery("#lblDownloadError").html(languageResource.resMsg_FailedToDownloadUserInfo);
                }
            }
            else {
                jQuery("#lblDownloadError").addClass("text-danger");
                jQuery("#lblDownloadError").html(languageResource.resMsg_FailedToDownloadUserInfo);
            }
        },
        beforeSend: function () {
            jQuery("#btnDownloadTemplate").attr("disabled", "disabled");
            

        },
        complete: function () {
        },
        error: function (request, error) {
            alert("error");
            
        }
    });
}

function TimerMethodsForDownloading(jsonValue) {
    var successtimer;
    var errortimer;
    var successTextFile = jsonValue + "_tempSuccess.txt";
    var errorTextFile = jsonValue + "_tempError.txt";
    var excelFileName = jsonValue + ".xlsx";
    successtimer = setInterval(function () {
        jQuery.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/CheckFileExist",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ fileName: successTextFile }),
            dataType: "json",
            success: function (json) {
                if (json.d != null) {
                    if (json.d != false) {
                        clearInterval(successtimer);
                        clearInterval(errortimer);
                        jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                        window.location = "../HandlerFiles/DownloadHandler.ashx?userInfoExcel=" + excelFileName;
                    }
                }
            },
            error: function (request, error) {
                clearInterval(successtimer);
                clearInterval(errortimer);
                
                jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                
            }
        });

    }, 10000);
    errortimer = setInterval(function () {
        jQuery.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/CheckFileExist",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ fileName: errorTextFile }),
            dataType: "json",
            success: function (json) {
                if (json.d != null) {
                    if (json.d != false) {
                        clearInterval(successtimer);
                        clearInterval(errortimer);
                        jQuery("#lblDownloadError").html(languageResource.resMsg_FailedToDownloadUserInfo);
                       
                        jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                        
                    }
                }
            },
            error: function (request, error) {
                clearInterval(successtimer);
                clearInterval(errortimer);
                jQuery("#btnDownloadTemplate").removeAttr("disabled", "disabled");
                
            }
        });
    }, 10000);
}
