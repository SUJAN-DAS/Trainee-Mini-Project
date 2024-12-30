﻿function ValidateForm() {
    let isValid = true;

    // Clear previous error messages
    $(".error-message").text("");

    const _firstName = jQuery("#txtFirstName").val().trim();
    const _lastName = jQuery("#txtLastName").val().trim();
    const _emailId = jQuery("#txtEmailID").val().trim();
    const _mobileNo = jQuery("#txtMobile").val().trim();
    const _password = jQuery("#txtPassword").val().trim();

    // Validate first name and last name
    if (_firstName === "") {
        jQuery("#firstNameError").text("First Name is required.");
        isValid = false;
    } else if (_firstName.length > 10) {
        jQuery("#firstNameError").text("First Name should not be more than 10 characters.");
        isValid = false;
    }

    if (_lastName === "") {
        jQuery("#LastNameError").text("Last Name is required.");
        isValid = false;
    } else if (_lastName.length > 10) {
        jQuery("#LastNameError").text("Last Name should not be more than 10 characters.");
        isValid = false;
    }

    // Validate email
    const emailRegEx = /^([\w-\.]+@(?!gmail.com)(?!yahoo.com)(?!hotmail.com)(?!yahoo.co.in)(?!yahoo.co)(?!aol.com)(?!abc.com)(?!xyz.com)(?!pqr.com)(?!rediffmail.com)(?!live.com)(?!outlook.com)(?!me.com)(?!msn.com)(?!ymail.com)([\w-]+\.)+[\w-]{2,4})?$/;
    if (_emailId === "") {
        jQuery("#EmailIdError").text("Email id is required.");
        isValid = false;
    } else if (!emailRegEx.test(_emailId)) {
        jQuery("#EmailIdError").text("Email is Invalid.");
        isValid = false;
    }

    // Validate mobile number
    const mobileRegEx = /^[6-9]\d{9}$/;
    if (_mobileNo === "") {
        jQuery("#MobileNoError").text("Mobile Number is required.");
        isValid = false;
    } else if (!mobileRegEx.test(_mobileNo)) {
        jQuery("#MobileNoError").text("Invalid Mobile Number.");
        isValid = false;
    }

    // Validate password
    const passwordRegEx = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@.#$!%*?&])[A-Za-z\d@.#$!%*?&]{8,15}$/;
    if (_password === "") {
        jQuery("#PasswordError").text("Password is required.");
        isValid = false;
    } else if (!passwordRegEx.test(_password)) {
        jQuery("#PasswordError").text("Password is Invalid.");
        isValid = false;
    }

    return isValid;
}


function ClearForm() {
    var form = document.getElementById("form1");
    if (form) {
        form.reset();
    }
}

function register(event) {
    event.preventDefault();
    if (ValidateForm()) {
        //alert("validation successfull");
        //ClearForm();
        const _firstName = document.getElementById("txtFirstName").value.trim();
        const _lastName = document.getElementById("txtLastName").value.trim();
        const _emailId = document.getElementById("txtEmailID").value.trim();
        const _MobileNo = document.getElementById("txtMobile").value.trim();
        const _password = document.getElementById("txtPassword").value.trim();
        /*const _selectedRole = document.querySelector("#roleDropdown").value;*/
        /*const _isAdmin = document.querySelector("#makeAdmin").checked*/
        $.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/RegisterDetails",
            data: JSON.stringify({
                firstName: _firstName,
                lastName: _lastName,
                emailId: _emailId ,
                mobileNo: _MobileNo,
                password: _password,
                /*role: _isAdmin?1:2*/
            }),
            contentType: "Application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log("Response:", response);
                if (response.d > 0) { 
                    alert("Registration Successful");
                    window.location.href = "Login.aspx";
                } else {
                    alert("Registration Failed");
                }

            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
                console.error("Response Text: " + xhr.responseText);
            }
        });
    }
    else {
        alert("Validation not successfull");
    }
}

function login(event) {
    event.preventDefault();
    const _emailId = jQuery("#txtEmail").val().trim();
    const _password = jQuery("#txtPassword").val().trim();

    if (!_emailId || !_password) {
        alert("Please enter both email and password.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/LoginDetails",
        data: JSON.stringify({
            emailId: _emailId,
            password: _password,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            const result = response.d;

                if (result.RoleId === 1) {
                    ClearLogin()
                    window.location.href = `Admin.aspx?userId=${result.UserId}`;
                } else if (result.RoleId === 2 || result.RoleId === 3) {
                    
                    ClearLogin()
                    window.location.href = `User.aspx?userId=${result.UserId}`;
                }
                else {
                    window.location.href = `FailedLoginAttempts.aspx?userId=${result.UserId}&email=${_emailId}&password=${_password}`;

                }
            
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            console.error("Response Text: " + xhr.responseText);
            alert("An error occurred while logging in. Please try again later.");
            window.location.href = `FailedLoginAttempts.aspx?userId=${result.UserId}`;
        }
    });
}
//trail logFailed
function logFailed(_emailId, _password) {
    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/LogInfo",
        data: JSON.stringify({
            _email:_emailId,
            _password:_password
        }),
        contentType: "Application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $("#failedLoginTable").find('tbody').append(response.d);
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            console.error("Response Text: " + xhr.responseText);
        }
    });



}

//trail logFailed
function UpdateUserInfo(userId) {
    Clear();
    $.ajax({
        type: "POST",

        url: "WebService/UserRegistration.asmx/GetUserInfo",

        data: JSON.stringify({

            userID: userId

        }),

        contentType: "Application/json; charset=utf-8",

        dataType: "json",

        success: function (response) {

            console.log("Response:", response);

            if (response.d.UserId > 0) {

                jQuery("#txtFirstName").val(response.d.FirstName);
                jQuery("#txtLastName").val(response.d.LastName);
                jQuery("#txtEmailID").val(response.d.Email);
                //trail
                //if (response.d.Role === 1) {
                //    jQuery('input[name="makeAdmin"]').prop('checked', true); // Check the box if role is Admin
                //} else {
                //    jQuery('input[name="makeAdmin"]').prop('checked', false); // Uncheck the box otherwise
                //}
                //switch (response.d.Role) {
                //    case 'Admin':
                //        jQuery("#roleDropdown").val("Admin");
                //        break;
                //    case 'Trainee':
                //        jQuery("#roleDropdown").val("Trainee");
                //        break;
                //    case 'Intern':
                //        jQuery("#roleDropdown").val("Intern");
                //        break;
                //    default:
                //        jQuery("#roleDropdown").val(""); // Set to empty or a default option
                //}

                /*alert('Edit function');*/
                //updateUserData(userId);
                //jQuery("#btnSubmit").attr('onclick', 'updateUserData(${userId})');
                //jQuery("#btnSubmit").text('Update Register');
            }

            else {

                alert("Registration Failed");

            }

        },

        error: function (xhr, status, error) {

            console.error("Error: " + error);

            console.error("Response Text: " + xhr.responseText);

        }

    });

}

function updateUserData(userId) {
    var userID = userId;
    var fName = jQuery("#txtFirstName").val();
    var lName = jQuery("#txtLastName").val();
    var emailId = jQuery("#txtEmailID").val();

   /* var admin = jQuery('input[name="makeAdmin"]').prop('checked') ? 1 : 2;*/
    /*var role = jQuery("#roleDropdown").val();*/


    var updatedUserInfo = {
        UserId: userID,
        FirstName: fName,
        LastName: lName,
        Email: emailId,
        /*Role: admin,*/

    };

    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/UpdateUserInfo",
        data: JSON.stringify({
            userInfo: updatedUserInfo
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log("Update Response:", response);

            if (response.d === true) {
                alert("User information updated successfully.");
                Clear();
                window.location.href = "Login.aspx";

            } else {
                alert("Failed to update user information.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error: " + error);
            console.error("Response Text: " + xhr.responseText);
            alert("An error occurred while updating user information.");
        }
    });
}

function Clear() {
    jQuery("#txtFirstName").val("");
    jQuery("#txtLastName").val("");
    jQuery("#txtEmailID").val("");
    jQuery("#txtMobile").val("");
    jQuery("#txtPassword").val("");
    jQuery('input[name="makeAdmin"]').prop('checked', false);
}

function ClearLogin() {
    jQuery("#txtEmail").val("");
    jQuery("#txtPassword").val("");
}