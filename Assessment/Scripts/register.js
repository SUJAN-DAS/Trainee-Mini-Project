function ValidateForm() {
    let isValid = true;

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
    const emailRegEx = /^([\w-\.]+@(?!hotmail\.com|aol\.com|rediffmail\.com|live\.com|outlook\.com|me\.com|msn\.com|ymail\.com|abc\.com|xyz\.com|pqr\.com)([\w-]+\.)+[\w-]{2,})$/;

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

function Register(event) {
    event.preventDefault();
    if (ValidateForm()) {
        //alert("validation successfull");
        //ClearForm();
        const firstName = document.getElementById("txtFirstName").value.trim();
        const lastName = document.getElementById("txtLastName").value.trim();
        const emailId = document.getElementById("txtEmailID").value.trim();
        const MobileNo = document.getElementById("txtMobile").value.trim();
        const password = document.getElementById("txtPassword").value.trim();
        const selectedRole = document.querySelector("#roleDropdown").value;
        /*const _isAdmin = document.querySelector("#makeAdmin").checked*/
        $.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/RegisterDetails",
            data: JSON.stringify({
                firstName: firstName,
                lastName: lastName,
                emailId: emailId ,
                mobileNo: MobileNo,
                password: password,
                role: selectedRole
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

function Login(event) {
    event.preventDefault();
    const emailId = jQuery("#txtEmail").val().trim();
    const password = jQuery("#txtPassword").val().trim();

    if (!emailId || !password) {
        alert("Please enter both email and password.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "WebService/UserRegistration.asmx/LoginDetails",
        data: JSON.stringify({
            emailId: emailId,
            password: password,
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
                    window.location.href = `FailedLoginAttempts.aspx?userId=${result.UserId}&email=${emailId}&password=${password}`;

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
//function logFailed(_emailId, _password) {
//    $.ajax({
//        type: "POST",
//        url: "WebService/UserRegistration.asmx/LogInfo",
//        data: JSON.stringify({
//            _email:_emailId,
//            _password:_password
//        }),
//        contentType: "Application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            $("#failedLoginTable").find('tbody').append(response.d);
//        },
//        error: function (xhr, status, error) {
//            console.error("Error: " + error);
//            console.error("Response Text: " + xhr.responseText);
//        }
//    });



//}

//trail logFailed
function UpdateUserInfo(userId) {
    Clear();
    $.ajax({
        type: "POST",

        url: "WebService/UserRegistration.asmx/GetUserDetail",

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

function UpdateUserData(userId) {
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
    /*jQuery('input[name="makeAdmin"]').prop('checked', false);*/
}

function ClearLogin() {
    jQuery("#txtEmail").val("");
    jQuery("#txtPassword").val("");
}
